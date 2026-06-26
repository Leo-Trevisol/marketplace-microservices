using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers {
    public class CarrinhoController : Controller {
        private readonly CarrinhoAPIService _carrinhoService;

        public CarrinhoController(CarrinhoAPIService carrinhoService) {
            _carrinhoService = carrinhoService;
        }

        [HttpPost]
        public IActionResult Adicionar(Guid produtoId, string codigo, string nome, decimal preco, int quantidade) {
            _carrinhoService.AdicionarItem(new CarrinhoModel {
                IdProduto = produtoId,
                Codigo = codigo,
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

        [HttpPost]
        public IActionResult Remover(Guid produtoId) {
            _carrinhoService.RemoverItem(produtoId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult FecharPedido() {
            var carrinho = _carrinhoService.ObterCarrinho();

            if (!carrinho.Any())
                return RedirectToAction("Index");

            // Aqui entra a lógica de criar o pedido (salvar no banco/API)
            // ex: _pedidoService.CriarPedido(carrinho);

            _carrinhoService.LimparCarrinho();

            return RedirectToAction("Index", "Pedido");
        }
    }
}
