using Ftec.ProjetoWeb.Produtos.Apresentacao.Facade;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
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
            var pedido = _apiFacade.ObterPedidoPorId(idPedido);
            if (pedido != null && pedido.Sucesso) {

                var model = new PagamentoModel
                {
                    Pedido = pedido.Data,
                };
                return View(model);
            } else {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public IActionResult Index(PagamentoModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var numeroPedido = new Random().Next(100000000, 999999999);

            TempData["NumeroPedido"] = numeroPedido.ToString();
            TempData["NomeCompleto"] = model.NomeCompleto;
            TempData["Email"] = model.Email;
            TempData["ProdutoNome"] = model.ProdutoNome;
            TempData["ProdutoPreco"] = model.ProdutoPreco.ToString("N2");
            TempData["ProdutoPrecoRaw"] = model.ProdutoPreco.ToString(System.Globalization.CultureInfo.InvariantCulture);
            TempData["FormaPagamento"] = model.FormaPagamento;
            TempData["Endereco"] = $"{model.Endereco}, {model.Numero}{(string.IsNullOrEmpty(model.Complemento) ? "" : " - " + model.Complemento)}, {model.Bairro}, {model.Cidade} - {model.Estado}, CEP {model.Cep}";

            var total = model.FormaPagamento == "pix"
                ? model.ProdutoPreco * 0.9m
                : model.ProdutoPreco;
            TempData["TotalFinal"] = total.ToString("N2");

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