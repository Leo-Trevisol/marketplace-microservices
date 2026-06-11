using Ftec.ProjetoWeb.Produtos.Apresentacao.Facade;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers
{
    public class ProdutoController : Controller
    {

        private readonly APIFacade _apiFacade;
        public ProdutoController(IConfiguration config)
        {
            //_apiFacade = new APIFacade();
        }
        public IActionResult Index()
        {
            try
            {
                //var produtos = _apiFacade.ListarProdutos();

                //return View(produtos);

                var produto = new ProdutoModel
                {
                    Id = Guid.NewGuid(),
                    Codigo = "PROD001",
                    Nome = "Produto Exemplo",
                    Preco = 99.99m,
                    QuantidadeEstoque = 10,
                    EstoqueMinimoVenda = 1,
                    IdCategoria = 1,
                    IdImagemPrincipal = Guid.NewGuid(),
                    Descricao = "Descrição do produto exemplo",
                    Destaque = true,
                    Disponivel = true,
                    Excluido = false,
                };
                ViewBag.ProdutosRelacionados = new List<ProdutoModel>
                {
                    new ProdutoModel
                    {
                        Id = Guid.NewGuid(),
                        Codigo = "PROD002",
                        Nome = "Smartphone Galaxy A56",
                        Preco = 1499.90m,
                        Disponivel = true
                    },
                    new ProdutoModel
                    {
                        Id = Guid.NewGuid(),
                        Codigo = "PROD003",
                        Nome = "Fone Bluetooth JBL",
                        Preco = 299.90m,
                        Disponivel = true
                    },
                    new ProdutoModel
                    {
                        Id = Guid.NewGuid(),
                        Codigo = "PROD004",
                        Nome = "Carregador Turbo USB-C",
                        Preco = 89.90m,
                        Disponivel = true
                    },
                    new ProdutoModel
                    {
                        Id = Guid.NewGuid(),
                        Codigo = "PROD005",
                        Nome = "Smartwatch Xiaomi",
                        Preco = 399.90m,
                        Disponivel = true
                    },
                    new ProdutoModel
                    {
                        Id = Guid.NewGuid(),
                        Codigo = "PROD005",
                        Nome = "Smartwatch Xiaomi",
                        Preco = 399.90m,
                        Disponivel = true
                    }
};

                return View(produto);


            }
            catch (Exception ex)
            {
                ViewBag.Erro = $"Erro ao carregar produtos: {ex.Message}";
                return View(new List<ProdutoModel>());
            }
        }

    }
}
