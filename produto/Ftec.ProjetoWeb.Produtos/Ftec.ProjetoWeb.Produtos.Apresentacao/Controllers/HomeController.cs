using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var produtos = new List<ProdutoModel>
            {
    new ProdutoModel
    {
        Id = Guid.NewGuid(),
        Codigo = "PRD001",
        Nome = "Notebook Gamer RTX 4060",
        Preco = 5499.90m,
        QuantidadeEstoque = 15,
        EstoqueMinimoVenda = 1,
        Descricao = "Notebook gamer com RTX 4060 e 16GB de RAM.",
        Destaque = true,
        Disponivel = true
    },
    new ProdutoModel
    {
        Id = Guid.NewGuid(),
        Codigo = "PRD002",
        Nome = "Mouse Gamer RGB",
        Preco = 149.90m,
        QuantidadeEstoque = 80,
        EstoqueMinimoVenda = 1,
        Descricao = "Mouse gamer RGB com 12000 DPI.",
        Destaque = true,
        Disponivel = true
    },
    new ProdutoModel
    {
        Id = Guid.NewGuid(),
        Codigo = "PRD003",
        Nome = "Teclado Mecânico Red Switch",
        Preco = 299.90m,
        QuantidadeEstoque = 40,
        EstoqueMinimoVenda = 1,
        Descricao = "Teclado mecânico ABNT2 com iluminação RGB.",
        Destaque = true,
        Disponivel = true
    },
    new ProdutoModel
    {
        Id = Guid.NewGuid(),
        Codigo = "PRD004",
        Nome = "Monitor 27'' Full HD",
        Preco = 899.90m,
        QuantidadeEstoque = 20,
        EstoqueMinimoVenda = 1,
        Descricao = "Monitor Full HD 75Hz de 27 polegadas.",
        Destaque = false,
        Disponivel = true
    },
    new ProdutoModel
    {
        Id = Guid.NewGuid(),
        Codigo = "PRD005",
        Nome = "Headset Gamer Surround",
        Preco = 249.90m,
        QuantidadeEstoque = 35,
        EstoqueMinimoVenda = 1,
        Descricao = "Headset com som surround e microfone removível.",
        Destaque = true,
        Disponivel = true
    },
    new ProdutoModel
    {
        Id = Guid.NewGuid(),
        Codigo = "PRD006",
        Nome = "Smartphone Android 256GB",
        Preco = 2299.90m,
        QuantidadeEstoque = 25,
        EstoqueMinimoVenda = 1,
        Descricao = "Smartphone com 256GB de armazenamento e câmera tripla.",
        Destaque = false,
        Disponivel = true
    },
    new ProdutoModel
    {
        Id = Guid.NewGuid(),
        Codigo = "PRD007",
        Nome = "Smart TV 55'' 4K",
        Preco = 3199.90m,
        QuantidadeEstoque = 12,
        EstoqueMinimoVenda = 1,
        Descricao = "Smart TV 4K com HDR e aplicativos integrados.",
        Destaque = true,
        Disponivel = true
    },
    new ProdutoModel
    {
        Id = Guid.NewGuid(),
        Codigo = "PRD008",
        Nome = "SSD NVMe 1TB",
        Preco = 449.90m,
        QuantidadeEstoque = 60,
        EstoqueMinimoVenda = 1,
        Descricao = "SSD NVMe PCIe de alta velocidade com 1TB.",
        Destaque = false,
        Disponivel = true
    },
    new ProdutoModel
    {
        Id = Guid.NewGuid(),
        Codigo = "PRD009",
        Nome = "Cadeira Gamer Ergonômica",
        Preco = 1299.90m,
        QuantidadeEstoque = 18,
        EstoqueMinimoVenda = 1,
        Descricao = "Cadeira gamer com ajuste de altura e reclinação.",
        Destaque = true,
        Disponivel = true
    },
    new ProdutoModel
    {
        Id = Guid.NewGuid(),
        Codigo = "PRD010",
        Nome = "Caixa de Som Bluetooth",
        Preco = 199.90m,
        QuantidadeEstoque = 50,
        EstoqueMinimoVenda = 1,
        Descricao = "Caixa de som portátil com bateria de longa duração.",
        Destaque = false,
        Disponivel = true
    }
};

            return View(produtos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
