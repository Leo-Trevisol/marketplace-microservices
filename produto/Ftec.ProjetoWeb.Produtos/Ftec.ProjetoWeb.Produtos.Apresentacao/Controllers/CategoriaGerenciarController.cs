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
                var categorias = _apiFacade.ListarCategoriasView();


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

        public IActionResult Editar(Guid id)
        {
            var modelo = _apiFacade.ObterCategoriaGerenciar(id.ToString());
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

        [HttpDelete]
        public IActionResult Excluir(Guid id)
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
            // 1. Busca a lista do Facade (aquela que já vem ordenada de forma hierárquica)
            var categorias = _apiFacade.ListarCategoriasView() ?? new List<CategoriaListaViewModel>();

            // 2. REGRA CRUCIAL: Se for Edição, a categoria não pode ser pai de si mesma!
            if (idCategoriaAtual.HasValue)
            {
                categorias = categorias.Where(c => c.Id != idCategoriaAtual.Value).ToList();
            }

            // 3. Formata o nome com espaços em branco especiais (Alt+0160) para o navegador não ignorar o recuo
            var listaFormatada = categorias.Select(c => new
            {
                Id = c.Id,
                Nome = c.Nivel > 0 ? $"{new string(' ', c.Nivel * 3)}↳ {c.Nome}" : c.Nome
            }).ToList();

            // 4. Insere a opção manual para Categoria Raiz (sem pai)
            listaFormatada.Insert(0, new { Id = 0, Nome = "[ Nenhuma - Categoria Raiz ]" });

            // 5. Transforma em SelectList e joga na ViewBag que a View está esperando
            ViewBag.ParentIdDropdown = new SelectList(listaFormatada, "Id", "Nome");
        }
    }
}
