using Ftec.ProjetoWeb.Produtos.Apresentacao.Facade;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers
{
    public class PagamentoController : Controller
    {
        private readonly CarrinhoAPIService _carrinhoService;
        private readonly APIFacade _apiFacade;
        public PagamentoController(IConfiguration config, CarrinhoAPIService carrinhoService) {
            _apiFacade = new APIFacade(config);
            _carrinhoService = carrinhoService;
        }
        [HttpGet]
        public IActionResult Index(Guid idPedido)
        {
            var carrinho = _carrinhoService.ObterCarrinho();

            if (!carrinho.Any())
                return RedirectToAction("Index", "Carrinho");

            decimal.TryParse(TempData.Peek("ValorFrete")?.ToString(),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out var valorFrete);

            var model = new PagamentoModel
            {
                PedidoId = idPedido,
                ValorFrete = valorFrete,
                Pedido = new APIPedidoModel
                {
                    id = idPedido,
                    valorTotal = carrinho.Sum(x => x.Preco * x.Quantidade),
                    produtosModel = carrinho.Select(x => new APIPedidoIntemModel
                    {
                        produtoId = Guid.Parse(x.IdProduto.ToString()),
                        preco = x.Preco,
                        quantidade = x.Quantidade
                    }).ToList()
                }
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(PagamentoModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var carrinho = _carrinhoService.ObterCarrinho();
            var subtotal = carrinho.Sum(x => x.Preco * x.Quantidade);
            var valorFrete = model.ValorFrete;

            var metodoPagamento = model.FormaPagamento switch
            {
                "credito" => 0,
                "pix" => 1,
                "boleto" => 2,
                _ => 0
            };

            var totalComFrete = subtotal + valorFrete;
            var total = model.FormaPagamento == "pix"
                ? totalComFrete * 0.9m
                : totalComFrete;

            try
            {
                var pagamentoModel = new APIPagamentoModel
                {
                    pedidoId = model.PedidoId,
                    cpfCliente = model.Cpf.Replace(".", "").Replace("-", ""),
                    valorTotal = total,
                    metodoPagamento = metodoPagamento
                };

                var pagamentoCriado = _apiFacade.RegistrarPagamento(pagamentoModel);

                if (pagamentoCriado != null && pagamentoCriado.pagamentoId != Guid.Empty)
                {
                    var transacao = _apiFacade.ProcessarTransacao(pagamentoCriado.pagamentoId, total);
                    TempData["StatusTransacao"] = transacao?.retornoGateway ?? "Pendente";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Aviso = ex.Message;
            }

            TempData["NumeroPedido"] = model.PedidoId.ToString();
            TempData["NomeCompleto"] = model.NomeCompleto;
            TempData["Email"] = model.Email;
            TempData["FormaPagamento"] = model.FormaPagamento;
            TempData["ValorFrete"] = valorFrete.ToString("N2");
            TempData["TotalFinal"] = total.ToString("N2");
            TempData["ProdutoPrecoRaw"] = subtotal.ToString(System.Globalization.CultureInfo.InvariantCulture);
            TempData["ProdutoPreco"] = subtotal.ToString("N2");
            TempData["Endereco"] = $"{model.Endereco}, {model.Numero}{(string.IsNullOrEmpty(model.Complemento) ? "" : " - " + model.Complemento)}, {model.Bairro}, {model.Cidade} - {model.Estado}, CEP {model.Cep}";

            _carrinhoService.LimparCarrinho();
            return RedirectToAction("Confirmacao");
        }

        public IActionResult Confirmacao()
        {
            if (TempData["NumeroPedido"] == null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        #region Functions
        #endregion
    }
}