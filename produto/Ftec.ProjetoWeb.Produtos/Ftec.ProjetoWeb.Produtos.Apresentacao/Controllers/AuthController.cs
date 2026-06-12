using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers
{
    public class AuthController : Controller
    {
        #region Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // autenticar usuário

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Cadastro

        [HttpGet]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(UsuarioModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // cadastrar usuário

            return RedirectToAction(nameof(Login));
        }

        #endregion

        public IActionResult Logout()
        {
            // logout

            return RedirectToAction("Index", "Home");
        }

    }
}
