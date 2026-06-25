using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

            var resultado = await _authApiService.CadastrarUsuarioAsync(model);

            if (resultado.Sucesso)
            {
                return RedirectToAction("Index", "Home");
            }

            // Tratamento de Erro da API
            if (!string.IsNullOrEmpty(resultado.Data))
            {
                try
                {
                    // Desserializa o JSON de erro da API
                    var apiErros = JsonSerializer.Deserialize<ApiErroResponse>(
                        resultado.Data,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    if (apiErros?.Erros != null && apiErros.Erros.Any())
                    {
                        foreach (var erro in apiErros.Erros)
                        {
                            // O ASP.NET Core mapeia os erros usando Case-Insensitive,
                            // então "Senha" ou "senha" vincularão corretamente ao input!
                            ModelState.AddModelError(erro.Campo ?? string.Empty, erro.Mensagem ?? "Valor inválido");
                        }
                        return View(model);
                    }
                }
                catch (JsonException)
                {
                    // Ignora erro de JSON e cai no erro genérico abaixo
                }
            }

            // Fallback: Se a API falhou mas não mandou a lista de erros esperada
            ModelState.AddModelError(string.Empty, "Não foi possível realizar o cadastro. Verifique os dados e tente novamente.");
            return View(model);
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