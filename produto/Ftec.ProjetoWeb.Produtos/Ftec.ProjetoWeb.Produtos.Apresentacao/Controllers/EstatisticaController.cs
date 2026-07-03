using Ftec.ProjetoWeb.Produtos.Apresentacao.Facade;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers {
    public class EstatisticaController : Controller{
        private readonly APIFacade _apiFacade;
        public EstatisticaController(IConfiguration config) {
            _apiFacade = new APIFacade(config);
        }

        public IActionResult Index(string? idProduto) {
            try {
                var model = new EstatisticaModel();

                return View(model);

            } catch (Exception ex) {
                ViewBag.Erro = $"Erro ao carregar estatísticas: {ex.Message}";
                return View(new List<ProdutoModel>());
            }
        }
    }
}
