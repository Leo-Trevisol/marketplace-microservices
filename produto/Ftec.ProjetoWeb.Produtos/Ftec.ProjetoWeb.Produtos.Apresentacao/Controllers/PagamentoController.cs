// PagamentoController.cs
using Ftec.ProjetoWeb.Produtos.Apresentacao.Facade;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers
{
    public class PagamentoController : Controller
    {
        private readonly CarrinhoAPIService _carrinhoService;
        private readonly APIFacade _apiFacade;
        public PagamentoController(IConfiguration config, CarrinhoAPIService carrinhoService)
        {
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

            Guid.TryParse(TempData.Peek("FreteId")?.ToString(), out var freteId);

            var model = new PagamentoModel
            {
                PedidoId = idPedido,
                ValorFrete = valorFrete,
                FreteId = freteId,
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

            var metodoPagamento = model.FormaPagamento switch
            {
                "credito" => 1, // CartaoCredito
                "pix" => 3,     // Pix
                "boleto" => 4,  // Boleto
                _ => 1
            };

            decimal total = 0;
            decimal valorFrete = model.ValorFrete; // fallback, caso a chamada falhe

            try
            {
                var pagamentoModel = new APIPagamentoModel
                {
                    pedidoId = model.PedidoId,
                    cpfCliente = model.Cpf.Replace(".", "").Replace("-", ""),
                    valorProdutos = subtotal,
                    metodoPagamento = metodoPagamento
                };

                var pagamentoCriado = _apiFacade.RegistrarPagamento(pagamentoModel);

                if (pagamentoCriado != null && pagamentoCriado.pagamentoId != Guid.Empty)
                {
                    valorFrete = pagamentoCriado.valorFrete; // valor real vindo da API

                    total = model.FormaPagamento == "pix"
                        ? pagamentoCriado.valorTotal * 0.9m
                        : pagamentoCriado.valorTotal;

                    var transacao = _apiFacade.ProcessarTransacao(pagamentoCriado.pagamentoId, total);
                    TempData["StatusTransacao"] = transacao?.retornoGateway ?? "Pendente";

                    if (transacao != null && transacao.statusTransacao && model.FreteId != Guid.Empty)
                    {
                        _apiFacade.ConfirmarFrete(model.FreteId);
                    }
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
    }
}