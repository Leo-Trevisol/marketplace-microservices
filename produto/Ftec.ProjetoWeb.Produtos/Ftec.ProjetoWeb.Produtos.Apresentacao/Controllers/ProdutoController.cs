using Microsoft.AspNetCore.Mvc;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers {
    public class ProdutoController : Controller {

        public IActionResult Index() {
            return View();
        }

    }
}
