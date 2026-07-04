using Ftec.ProjetoWeb.Produtos.Apresentacao.Facade;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.Text.Json;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers
{
    public class ProdutoGerenciarController : Controller
    {
        private readonly APIFacade _apiFacade;
        public ProdutoGerenciarController(IConfiguration config)
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
        public IActionResult Cadastrar()
        {
            var modelo = new APIProdutoModel();
            CarregarCategoriasNoDropdown();

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(APIProdutoModel model, IFormFile imagem)
        {
            try
            {
                if (model.Id == Guid.Empty)
                {
                    model.Id = Guid.NewGuid();
                }

                // Faz upload da imagem, caso tenha sido enviada
                if (imagem != null && imagem.Length > 0)
                {
                    var media = await _apiFacade.UploadImagemAsync(imagem);

                    if (media == null)
                    {
                        ViewBag.Erro = "Erro ao realizar o upload da imagem.";
                        CarregarCategoriasNoDropdown(model.IdCategoria);
                        return View(model);
                    }
                    model.IdImagemPrincipal = media.Id;
                }


                var status = _apiFacade.AdicionarProduto(model);

                if (status)
                {
                    TempData["Sucesso"] = $"Produto \"{model.Nome}\" cadastrado com sucesso!";
                    TempData["StatusApi"] = "sucesso";
                }
                else
                {
                    TempData["Erro"] = $"Erro ao cadastrar o produto \"{model.Nome}\".";
                }

                return RedirectToAction("Confirmacao");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = $"Erro ao cadastrar produto: {ex.Message}";
                CarregarCategoriasNoDropdown(model.IdCategoria);
                return View(model);
            }
        }

        public IActionResult Editar(Guid id)
        {
            var modelo = _apiFacade.ObterProdutoGerenciar(id.ToString());
            CarregarCategoriasNoDropdown(modelo.IdCategoria);

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(APIProdutoModel model, IFormFile? imagem)
        {
            try
            {
                if (model.Id == Guid.Empty)
                {
                    TempData["Erro"] = "Produto inválido.";
                    return RedirectToAction("Index");
                }

                // Upload de nova imagem (opcional)
                if (imagem != null && imagem.Length > 0)
                {
                    var media = await _apiFacade.UploadImagemAsync(imagem);

                    if (media == null)
                    {
                        ViewBag.Erro = "Erro ao realizar upload da imagem.";
                        CarregarCategoriasNoDropdown(model.IdCategoria);
                        return View(model);
                    }

                    model.IdImagemPrincipal = media.Id;
                }

                var status = _apiFacade.AlterarProduto(model);

                if (status)
                {
                    TempData["Sucesso"] = $"Produto \"{model.Nome}\" atualizado com sucesso!";
                    TempData["StatusApi"] = "sucesso";
                }
                else
                {
                    TempData["Erro"] = $"Erro ao atualizar produto \"{model.Nome}\".";
                }

                return RedirectToAction("Confirmacao");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = $"Erro ao editar produto: {ex.Message}";
                CarregarCategoriasNoDropdown(model.IdCategoria);
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Excluir(Guid id)
        {
            var status = _apiFacade.ExcluirProduto(id);
            if (status)
            {
                TempData["Sucesso"] = $"Produto excluído com sucesso!";
                TempData["StatusApi"] = "sucesso";
                return RedirectToAction("Confirmacao");
            }
            else
            {
                TempData["Erro"] = $"Erro ao excluir produto!";
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
                lista = lista.ToList();
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
                Nome = "Selecione uma categoria"
            });

            if (idCategoriaAtual == null)
            {

                ViewBag.ParentIdDropdown = new SelectList(listaFormatada, "Id", "Nome");
            }
            else
            {
                ViewBag.ParentIdDropdown = new SelectList(
                    listaFormatada,
                    "Id",
                    "Nome",
                    idCategoriaAtual
                );
            }
        }

    }
}
