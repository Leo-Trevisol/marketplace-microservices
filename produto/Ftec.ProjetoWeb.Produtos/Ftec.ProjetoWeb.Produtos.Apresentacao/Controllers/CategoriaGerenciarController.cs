using Ftec.ProjetoWeb.Produtos.Apresentacao.Facade;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers
{
    public class CategoriaGerenciarController : Controller
    {
        private readonly APIFacade _apiFacade;

        public CategoriaGerenciarController(IConfiguration config)
        {
            _apiFacade = new APIFacade(config);
        }
        public IActionResult Index()
        {
            try
            {
                var categorias = _apiFacade.ListarCategorias();


                return View(categorias);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = $"Erro ao carregar categorias: {ex.Message}";
                return View(new List<CategoriaModel>());
            }
        }


        public IActionResult Cadastrar()
        {
            CarregarCategoriasNoDropdown();

            var modelo = new APICategoriaModel();
            return View(modelo);
        }

        [HttpPost]
        public IActionResult Cadastrar(APICategoriaModel model)
        {
            try
            {
                if (model.ParentId == 0)
                {
                    model.ParentId = null;
                }

                var status = _apiFacade.AdicionarCategoria(model);

                if (status)
                {
                    TempData["Sucesso"] = $"Categoria \"{model.Nome}\" cadastrada com sucesso!";
                    TempData["StatusApi"] = "sucesso";
                    return RedirectToAction("Confirmacao");
                }
                else
                {
                    TempData["Erro"] = $"Categoria \"{model.Nome}\" erro ao cadastrar categoria!";
                    return RedirectToAction("Confirmacao");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Erro = $"Erro ao cadastrar categoria: {ex.Message}";

                // 🚀 ADICIONE AQUI: Alimenta o select se a tela precisar ser recarregada por erro
                CarregarCategoriasNoDropdown();

                return View(model);
            }
        }

        public IActionResult Editar(int id)
        {
            var modelo = _apiFacade.ObterCategoriaGerenciar(id);
            if (modelo == null) return NotFound();

            CarregarCategoriasNoDropdown(modelo.Id);
            return View(modelo);
        }

        [HttpPost]
        public IActionResult Editar(APICategoriaModel model)
        {
            var status = _apiFacade.AlterarCategoria(model);
            if (status)
            {
                TempData["Sucesso"] = $"Categoria \"{model.Nome}\" editada com sucesso!";
                TempData["StatusApi"] = "sucesso";
                return RedirectToAction("Confirmacao");
            }
            else
            {
                TempData["Erro"] = $"Categoria \"{model.Nome}\" erro ao editar categoria!";
                return RedirectToAction("Confirmacao");

            }
        }

        [HttpPost]
        public IActionResult Excluir(int id)
        {
            var status = _apiFacade.ExcluirCategoria(id);
            if (status)
            {
                TempData["Sucesso"] = $"Categoria excluída com sucesso!";
                TempData["StatusApi"] = "sucesso";
                return RedirectToAction("Confirmacao");
            }
            else
            {
                TempData["Erro"] = $"Erro ao excluir categoria!";
                return RedirectToAction("Confirmacao");

            }
        }

        public IActionResult Confirmacao()
        {
            return View();
        }


        private void CarregarCategoriasNoDropdown(int? idCategoriaAtual = null)
        {
            var arvore = _apiFacade.ListarCategorias() ?? new List<CategoriaArvoreModel>();

            var lista = new List<CategoriaListaViewModel>();

            void AdicionarCategorias(IEnumerable<CategoriaArvoreModel> categorias, int nivel)
            {
                foreach (var categoria in categorias)
                {
                    lista.Add(new CategoriaListaViewModel
                    {
                        Id = categoria.Id,
                        Nome = categoria.Nome,
                        Nivel = nivel
                    });

                    if (categoria.Filhos.Any())
                    {
                        AdicionarFilhos(categoria.Filhos, nivel + 1);
                    }
                }
            }

            void AdicionarFilhos(IEnumerable<CategoriaModel> filhos, int nivel)
            {
                foreach (var filho in filhos)
                {
                    lista.Add(new CategoriaListaViewModel
                    {
                        Id = filho.Id,
                        Nome = filho.Nome,
                        Nivel = nivel
                    });
                }
            }

            AdicionarCategorias(arvore, 0);

            if (idCategoriaAtual.HasValue)
            {
                lista = lista.Where(c => c.Id != idCategoriaAtual.Value).ToList();
            }

            var listaFormatada = lista.Select(c => new
            {
                Id = c.Id,
                Nome = c.Nivel > 0
                    ? $"{new string('\u00A0', c.Nivel * 4)}↳ {c.Nome}"
                    : c.Nome
            }).ToList();

            listaFormatada.Insert(0, new
            {
                Id = 0,
                Nome = "[ Nenhuma - Categoria Raiz ]"
            });

            ViewBag.ParentIdDropdown = new SelectList(listaFormatada, "Id", "Nome");
        }

    }
}
