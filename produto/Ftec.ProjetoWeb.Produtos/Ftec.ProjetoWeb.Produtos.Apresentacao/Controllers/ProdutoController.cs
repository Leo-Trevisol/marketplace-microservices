using Ftec.ProjetoWeb.Produtos.Apresentacao.Facade;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers
{
    public class ProdutoController : Controller
    {

        private readonly APIFacade _apiFacade;
        public ProdutoController(IConfiguration config)
        {
            _apiFacade = new APIFacade(config);
        }
        public IActionResult Index()
        {
            try
            {
                var produtos = _apiFacade.ListarProdutos();
                return View(produtos);


            }
            catch (Exception ex)
            {
                ViewBag.Erro = $"Erro ao carregar produtos: {ex.Message}";
                return View(new List<ProdutoModel>());
            }
        }
        public IActionResult Detalhe(string idProduto) {
            try {
                var produto = _apiFacade.ObterProduto(idProduto);
                return View(produto);


            } catch (Exception ex) {
                ViewBag.Erro = $"Erro ao carregar produto: {ex.Message}";
                return View(new List<ProdutoModel>());
            }
        }
    }
}
