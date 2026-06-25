using Ftec.ProjetoWeb.Produtos.Apresentacao.Facade;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API;
using Microsoft.AspNetCore.Mvc;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers
{
    public class ProdutoController : Controller
    {

        private readonly APIFacade _apiFacade;
        public ProdutoController(IConfiguration config)
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
        public IActionResult Detalhe(string idProduto)
        {
            try
            {
                var produto = _apiFacade.ObterProduto(idProduto);
                produto.Relacionados = this.ObterProdutosRelacionados(produto.IdCategoria.Value, produto.Id);
                return View(produto);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = $"Erro ao carregar produto: {ex.Message}";
                return View(new List<ProdutoModel>());
            }
        }
        public APIResponseModel<ProdutoAvaliacaoModel> RegistrarAvaliacao(ProdutoAvaliacaoModel model) {
            var response = new APIResponseModel<ProdutoAvaliacaoModel>();
            try {
                if (model.Id == Guid.Empty) {
                    model.Id = Guid.NewGuid();
                }
                response.Sucesso = _apiFacade.AdicionarAvaliacao(model);
                response.Message = response != null && response.Sucesso
                    ? "Sucesso ao registrar avaliação!"
                    : "Não foi possível registrar avaliação. Tente novamente!";
                return response;
            } catch (Exception ex) {
                response.Sucesso = false;
                response.Message = $"ERRO! {ex.Message}";
                return response;
            }

        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(CadastroProdutoModel model)
        {
            try
            {
                var produto = new ProdutoModel
                {
                    Id = Guid.NewGuid(),
                    Codigo = model.Codigo,
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    Preco = model.Preco,
                    QuantidadeEstoque = model.QuantidadeEstoque,
                    EstoqueMinimoVenda = model.EstoqueMinimoVenda,
                    IdCategoria = model.IdCategoria,
                    Disponivel = model.Disponivel,
                    Destaque = model.Destaque,
                    Excluido = false
                };

                _apiFacade.AdicionarProduto(produto);

                TempData["Sucesso"] = $"Produto \"{model.Nome}\" cadastrado com sucesso!";
                return RedirectToAction("CadastroConfirmado");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = $"Erro ao cadastrar produto: {ex.Message}";
                return View(model);
            }
        }

        public IActionResult CadastroConfirmado()
        {
            return View();
        }

        #region Functions
        [NonAction]
        public List<ProdutoModel> ObterProdutosRelacionados(int idCategoria, Guid idProduto) {
            var relacionados = new List<ProdutoModel>();
            var produtos = _apiFacade.ListarProdutos();
            if (produtos != null && produtos.Count() > 0) {
                foreach (var item in produtos) {
                    if (item.IdCategoria == idCategoria && item.Id != idProduto) {
                        relacionados.Add(item);
                    } else {
                        continue;
                    }
                }
            }
            return relacionados;
        }
        #endregion
    }
}
