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
                //var categorias = _apiFacade.ListarCategoriasView();
                var categorias = new List<CategoriaListaViewModel>
                {
                    // Raiz: Eletrônicos (Nível 0)
                    new CategoriaListaViewModel { Id = 1, Nome = "Eletrônicos", Descricao = "Aparelhos e dispositivos eletrônicos", ParentId = null, Nivel = 0, CategoriaPaiNome = "Nenhum" },
        
                    // Filhos de Eletrônicos (Nível 1)
                    new CategoriaListaViewModel { Id = 2, Nome = "Computadores", Descricao = "Notebooks, desktops e servidores", ParentId = 1, Nivel = 1, CategoriaPaiNome = "Eletrônicos" },
                    new CategoriaListaViewModel { Id = 3, Nome = "Smartphones", Descricao = "Celulares e smart devices", ParentId = 1, Nivel = 1, CategoriaPaiNome = "Eletrônicos" },
        
                    // Netos de Eletrônicos / Filhos de Smartphones (Nível 2)
                    new CategoriaListaViewModel { Id = 4, Nome = "Android", Descricao = "Dispositivos com sistema operacional Google", ParentId = 3, Nivel = 2, CategoriaPaiNome = "Smartphones" },
                    new CategoriaListaViewModel { Id = 5, Nome = "iOS", Descricao = "Dispositivos do ecossistema Apple", ParentId = 3, Nivel = 2, CategoriaPaiNome = "Smartphones" },
        
                    // Raiz: Moda (Nível 0)
                    new CategoriaListaViewModel { Id = 6, Nome = "Moda", Descricao = "Vestuário, calçados e acessórios", ParentId = null, Nivel = 0, CategoriaPaiNome = "Nenhum" },
        
                    // Filhos de Moda (Nível 1)
                    new CategoriaListaViewModel { Id = 7, Nome = "Calçados", Descricao = "Tênis, sapatos e botas", ParentId = 6, Nivel = 1, CategoriaPaiNome = "Moda" },
                    new CategoriaListaViewModel { Id = 8, Nome = "Vestuário", Descricao = "Camisas, calças e casacos", ParentId = 6, Nivel = 1, CategoriaPaiNome = "Moda" }
                };

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
