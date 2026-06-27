using Ftec.ProjetoWeb.Produtos.Apresentacao.Facade;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers {
    public class ProdutoGerenciarController : Controller{
        private readonly APIFacade _apiFacade;
        public ProdutoGerenciarController(IConfiguration config) {
            _apiFacade = new APIFacade(config);
        }
        public IActionResult Index() {
            try {
                var produtos = _apiFacade.ListarProdutos();
                return View(produtos);

            } catch (Exception ex) {
                ViewBag.Erro = $"Erro ao carregar produtos: {ex.Message}";
                return View(new List<ProdutoModel>());
            }
        }
        public IActionResult Cadastrar() {
            var modelo = new APIProdutoModel();
            return View(modelo);
        }

        [HttpPost]
        public IActionResult Cadastrar(APIProdutoModel model) {
            try {
                if(model.Id == Guid.Empty) {
                    model.Id = Guid.NewGuid();
                }
                if(model.IdImagemPrincipal == Guid.Empty) {
                    model.IdImagemPrincipal = Guid.NewGuid();
                }
                var status = _apiFacade.AdicionarProduto(model);
                if (status){
                    TempData["Sucesso"] = $"Produto \"{model.Nome}\" cadastrado com sucesso!";
                    TempData["StatusApi"] = "sucesso";
                    return RedirectToAction("Confirmacao");
                } else {
                    TempData["Erro"] = $"Produto \"{model.Nome}\" erro ao cadastrar produto!";
                    return RedirectToAction("Confirmacao");

                }
            } catch (Exception ex) {
                ViewBag.Erro = $"Erro ao cadastrar produto: {ex.Message}";
                return View(model);
            }
        }

        public IActionResult Editar(Guid id) {
            var modelo = _apiFacade.ObterProdutoGerenciar(id.ToString());
            return View(modelo);
        }

        [HttpPost]
        public IActionResult Editar(APIProdutoModel model) {
            var status = _apiFacade.AlterarProduto(model);
            if (status) {
                TempData["Sucesso"] = $"Produto \"{model.Nome}\" editado com sucesso!";
                TempData["StatusApi"] = "sucesso";
                return RedirectToAction("Confirmacao");
            } else {
                TempData["Erro"] = $"Produto \"{model.Nome}\" erro ao editar produto!";
                return RedirectToAction("Confirmacao");

            }
        }

        [HttpPost]
        public IActionResult Excluir(Guid id) {
            var status = _apiFacade.ExcluirProduto(id);
            if (status) {
                TempData["Sucesso"] = $"Produto excluído com sucesso!";
                TempData["StatusApi"] = "sucesso";
                return RedirectToAction("Confirmacao");
            } else {
                TempData["Erro"] = $"Erro ao excluir produto!";
                return RedirectToAction("Confirmacao");

            }
        }

        public IActionResult Confirmacao() {
            return View();
        }

    }
}
