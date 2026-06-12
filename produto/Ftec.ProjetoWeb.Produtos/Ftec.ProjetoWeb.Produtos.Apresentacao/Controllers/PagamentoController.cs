using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers
{
    public class PagamentoController : Controller
    {
        [HttpGet]
        public IActionResult Index(int codigo, string nome, decimal preco)
        {
            var model = new PagamentoModel
            {
                ProdutoCodigo = codigo,
                ProdutoNome = nome,
                ProdutoPreco = preco
            };

            return View(model);
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
    }
}