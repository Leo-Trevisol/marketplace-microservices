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

            var model = new PagamentoModel
            {
                PedidoId = idPedido,
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
            var valorTotal = carrinho.Sum(x => x.Preco * x.Quantidade);

            var metodoPagamento = model.FormaPagamento switch
            {
                "credito" => 0,
                "pix" => 1,
                "boleto" => 2,
                _ => 0
            };

            var total = model.FormaPagamento == "pix"
                ? valorTotal * 0.9m
                : valorTotal;

            try
            {
                var pagamentoModel = new APIPagamentoModel
                {
                    pedidoId = model.PedidoId,
                    cpfCliente = model.Cpf.Replace(".", "").Replace("-", ""),
                    valorTotal = total,
                    metodoPagamento = metodoPagamento
                };

                _apiFacade.RegistrarPagamento(pagamentoModel);
            }
            catch (Exception ex)
            {
                ViewBag.Aviso = ex.Message;
            }
            TempData["NumeroPedido"] = model.PedidoId.ToString();
            TempData["NomeCompleto"] = model.NomeCompleto;
            TempData["Email"] = model.Email;
            TempData["FormaPagamento"] = model.FormaPagamento;
            TempData["TotalFinal"] = total.ToString("N2");
            TempData["ProdutoPrecoRaw"] = valorTotal.ToString(System.Globalization.CultureInfo.InvariantCulture);
            TempData["ProdutoPreco"] = valorTotal.ToString("N2");
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