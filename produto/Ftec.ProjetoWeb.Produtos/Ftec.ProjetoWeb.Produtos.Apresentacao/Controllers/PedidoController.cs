using Ftec.ProjetoWeb.Produtos.Apresentacao.Facade;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers
{
    public class PedidoController : Controller {
        private readonly CarrinhoAPIService _carrinhoService;
        private readonly APIFacade _apiFacade;
        public PedidoController(IConfiguration config, CarrinhoAPIService carrinhoService) {
            _apiFacade = new APIFacade(config);
            _carrinhoService = carrinhoService;
        }
        [HttpPost]
        public IActionResult RegistrarPedido(Guid usuarioId) {
            var response = new APIResponseModel<APIPedidoRegistrarModel>();
            var carrinho = _carrinhoService.ObterCarrinho();

            if (!carrinho.Any())
                return RedirectToAction("Index", "Carrinho");
            try {
                var pedido = new APIPedidoRegistrarModel {
                    id = Guid.NewGuid(),
                    usuarioId = usuarioId,
                    produtosModel = carrinho.Select(item => new APIPedidoProdutoModel {
                        produtoId = Guid.Parse(item.IdProduto.ToString()),
                        quantidade = item.Quantidade,
                    }).ToList(),
                    cepEnderecoEntrega = "9999-999",
                    numeroEnderecoEntrega = "00"
                };

                response.Sucesso = _apiFacade.AdicionarPedido(pedido);

                if (response.Sucesso) {
                    return RedirectToAction("Index", "Pagamento", new {
                        idPedido = pedido.id
                    });
                } else {
                    return RedirectToRoute("carrinho/index");
                }

            } catch (Exception ex) {
                return RedirectToRoute("carrinho/index");
            }
        }
        //[HttpGet]
        //public IActionResult MeusPedidos()
        //{
        //    // ─────────────────────────────────────────────────────────
        //    // var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    // var pedidos = await _pedidoService.ListarPorUsuario(usuarioId);
        //    // return View(pedidos);
        //    // ─────────────────────────────────────────────────────────

        //    var pedidos = ObterPedidosFixos();
        //    return View(pedidos);
        //}

        //[HttpGet]
        //public IActionResult Detalhe(Guid numero)
        //{

        //    var pedido = _apiFacade.ObterPedidoPorId(numero);

        //    if (pedido == null && pedido.Sucesso)
        //        return RedirectToAction(nameof(MeusPedidos));

        //    return View(pedido);
        //}

        
    //    private static List<PedidoModel> ObterPedidosFixos()
    //    {
    //        return new List<PedidoModel>
    //{
    //    new PedidoModel
    //    {
    //        Id            = 1,
    //        NumeroPedido  = "123456789",
    //        DataPedido    = DateTime.Now.AddDays(-2),
    //        Status        = "Pago",
    //        NomeCompleto  = "João Silva",
    //        Email         = "joao@email.com",
    //        Telefone      = "(11) 99999-0001",
    //        Endereco      = "Rua das Flores",
    //        Numero        = "100",
    //        Complemento   = "Apto 12",
    //        Bairro        = "Centro",
    //        Cidade        = "São Paulo",
    //        Estado        = "SP",
    //        Cep           = "01000-000",
    //        FormaPagamento = "pix",
    //        TotalPago     = 3599.10m,
    //        Itens = new List<PedidoItemModel>
    //        {
    //            new PedidoItemModel
    //            {
    //                ProdutoCodigo = 1,
    //                ProdutoNome   = "iPhone 13 128GB Meia-Noite",
    //                PrecoUnitario = 3999.00m,
    //                Quantidade    = 1,
    //                Subtotal      = 3599.10m
    //            }
    //        }
    //    },
    //    new PedidoModel
    //    {
    //        Id            = 2,
    //        NumeroPedido  = "987654321",
    //        DataPedido    = DateTime.Now.AddDays(-10),
    //        Status        = "Entregue",
    //        NomeCompleto  = "João Silva",
    //        Email         = "joao@email.com",
    //        Telefone      = "(11) 99999-0001",
    //        Endereco      = "Rua das Flores",
    //        Numero        = "100",
    //        Complemento   = "",
    //        Bairro        = "Centro",
    //        Cidade        = "São Paulo",
    //        Estado        = "SP",
    //        Cep           = "01000-000",
    //        FormaPagamento = "credito",
    //        TotalPago     = 9999.00m,
    //        Itens = new List<PedidoItemModel>
    //        {
    //            new PedidoItemModel
    //            {
    //                ProdutoCodigo = 2,
    //                ProdutoNome   = "MacBook Pro M3",
    //                PrecoUnitario = 9999.00m,
    //                Quantidade    = 1,
    //                Subtotal      = 9999.00m
    //            }
    //        }
    //    },
    //    new PedidoModel
    //    {
    //        Id            = 3,
    //        NumeroPedido  = "456789123",
    //        DataPedido    = DateTime.Now.AddDays(-1),
    //        Status        = "Aguardando pagamento",
    //        NomeCompleto  = "João Silva",
    //        Email         = "joao@email.com",
    //        Telefone      = "(11) 99999-0001",
    //        Endereco      = "Rua das Flores",
    //        Numero        = "100",
    //        Complemento   = "",
    //        Bairro        = "Centro",
    //        Cidade        = "São Paulo",
    //        Estado        = "SP",
    //        Cep           = "01000-000",
    //        FormaPagamento = "boleto",
    //        TotalPago     = 299.90m,
    //        Itens = new List<PedidoItemModel>
    //        {
    //            new PedidoItemModel
    //            {
    //                ProdutoCodigo = 3,
    //                ProdutoNome   = "Teclado Mecânico Redragon",
    //                PrecoUnitario = 299.90m,
    //                Quantidade    = 1,
    //                Subtotal      = 299.90m
    //            }
    //        }
    //    }
    //};
    //}
    }
}