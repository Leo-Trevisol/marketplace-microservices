using Ftec.ProjetoWeb.Produtos.Apresentacao.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers {
    public class CarrinhoController : Controller {
        private readonly CarrinhoAPIService _carrinhoService;

        public CarrinhoController(CarrinhoAPIService carrinhoService) {
            _carrinhoService = carrinhoService;
        }

        [HttpPost]
        public IActionResult Adicionar(Guid produtoId, string nome, decimal preco, int quantidade) {
            _carrinhoService.AdicionarItem(new CarrinhoItem {
                IdProduto = produtoId,
                Nome = nome,
                Preco = preco,
                Quantidade = quantidade
            });

            return RedirectToAction("Index");
        }

        public IActionResult Index() {
            var carrinho = _carrinhoService.ObterCarrinho();
            return View(carrinho);
        }
    }
}
