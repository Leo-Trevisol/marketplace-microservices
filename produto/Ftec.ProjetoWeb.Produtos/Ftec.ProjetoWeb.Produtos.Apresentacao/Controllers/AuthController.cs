using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthApiService _authApiService;

        public AuthController(AuthApiService authApiService)
        {
            _authApiService = authApiService;
        }

        #region Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authApiService.LoginAsync(model.Email, model.Senha);

            if (result == null)
            {
                ViewBag.Erro = "E-mail ou senha inválidos.";
                return View(model);
            }

            HttpContext.Session.SetString("AccessToken", result.AccessToken);
            HttpContext.Session.SetString("RefreshToken", result.RefreshToken);
            HttpContext.Session.SetString("UsuarioId", result.UsuarioId.ToString());
            HttpContext.Session.SetString("Nome", result.Nome);
            HttpContext.Session.SetString("Email", result.Email);

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
        public async Task<IActionResult> Cadastro(UsuarioModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resultado = await _authApiService
                .CadastrarUsuarioAsync(model);

            return Content(resultado);
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = HttpContext.Session.GetString("RefreshToken");

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _authApiService.LogoutAsync(refreshToken);
            }

            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Auth");
        }


        [HttpGet]
        public IActionResult FakeLogin()
        {
            HttpContext.Session.SetString("AccessToken", "fake-token");
            HttpContext.Session.SetString("RefreshToken", "fake-refresh");
            HttpContext.Session.SetString("UsuarioId", Guid.NewGuid().ToString());
            HttpContext.Session.SetString("Nome", "Usuario Teste");
            HttpContext.Session.SetString("Email", "teste@local.com");

            return RedirectToAction("Index", "Home");
        }
    }
}