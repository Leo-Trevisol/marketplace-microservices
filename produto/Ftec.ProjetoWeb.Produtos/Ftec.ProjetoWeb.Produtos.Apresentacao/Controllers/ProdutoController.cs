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
        public IActionResult Index(string? texto, string? idCategoria = null)
        {
            try
            {

                List<ProdutoModel> produtos;
                var categorias = _apiFacade.ListarGeralCategorias();
                ViewBag.Categorias = categorias;
                if (!string.IsNullOrWhiteSpace(texto))
                {
                    produtos = _apiFacade.BuscarProdutosPorTexto(texto);
                }
                else
                {
                    produtos = _apiFacade.ListarProdutos();
                    if (!string.IsNullOrEmpty(idCategoria))
                    {
                        long.TryParse(idCategoria, out long categoriaId);
                        if (produtos != null && produtos.Count > 0)
                        {
                            produtos = this.ObterPorCategoria(produtos, categoriaId);
                        }
                    }
                }

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
                return View(new ProdutoModel());
            }
        }
        public APIResponseModel<APIProdutoAvaliacaoModel> RegistrarAvaliacao(APIProdutoAvaliacaoModel model)
        {
            var response = new APIResponseModel<APIProdutoAvaliacaoModel>();
            try
            {
                if (model.Id == Guid.Empty)
                {
                    model.Id = Guid.NewGuid();
                }
                response.Sucesso = _apiFacade.AdicionarAvaliacao(model);
                response.Message = response != null && response.Sucesso
                    ? "Sucesso ao registrar avaliação!"
                    : "Não foi possível registrar avaliação. Tente novamente!";
                return response;
            }
            catch (Exception ex)
            {
                response.Sucesso = false;
                response.Message = $"ERRO! {ex.Message}";
                return response;
            }

        }

        #region Functions
        [NonAction]
        public List<ProdutoModel> ObterProdutosRelacionados(int idCategoria, Guid idProduto)
        {
            var relacionados = new List<ProdutoModel>();
            var produtos = _apiFacade.ListarProdutos();
            if (produtos != null && produtos.Count() > 0)
            {
                foreach (var item in produtos)
                {
                    if (item.IdCategoria == idCategoria && item.Id != idProduto)
                    {
                        relacionados.Add(item);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return relacionados;
        }
        [NonAction]
        public List<ProdutoModel> ObterPorCategoria(List<ProdutoModel> produtos, long idCategoria)
        {
            var lista = new List<ProdutoModel>();
            foreach (var produto in produtos)
            {
                if (produto.IdCategoria == idCategoria)
                {
                    lista.Add(produto);
                }
            }

            return lista;
        }
        #endregion
    }
}
