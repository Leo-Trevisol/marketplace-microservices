using Ftec.ProjetoWeb.Produtos.Apresentacao.Facade;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers
{
    public class HomeController : Controller
    {
        private readonly APIFacade _apiFacade;
        public HomeController(IConfiguration config)
        {
            _apiFacade = new APIFacade(config);
        }
        public IActionResult Index()
        {
            var model = new HomeModel();
            model.ProdutosDestaque = this.ObterProdutosDestaque();

            model.Categorias = this.ObterCategorias();

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Functions
        [NonAction]
        public List<ProdutoModel> ObterProdutosDestaque()
        {
            var destaques = new List<ProdutoModel>();
            var produtos = _apiFacade.ListarProdutos();
            if (produtos != null && produtos.Count() > 0)
            {
                foreach (var item in produtos)
                {
                    if (item.Destaque)
                    {
                        destaques.Add(item);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            return destaques;
        }
        public List<CategoriaModel> ObterCategorias()
        {
            var categorias = _apiFacade.ListarGeralCategorias();
            if (categorias != null && categorias.Count() > 0)
            {
                return categorias;
            }
            else
            {
                return new List<CategoriaModel>();
            }
        }
        #endregion
    }
}
