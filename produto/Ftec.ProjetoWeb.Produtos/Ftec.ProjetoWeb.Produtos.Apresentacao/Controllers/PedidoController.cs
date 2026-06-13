using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers
{
    public class PedidoController : Controller
    {
        // ─────────────────────────────────────────────────────────────
        // private readonly IPedidoService _pedidoService;
        // public PedidoController(IPedidoService pedidoService)
        // {
        //     _pedidoService = pedidoService;
        // }
        // ─────────────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult MeusPedidos()
        {
            // ─────────────────────────────────────────────────────────
            // var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // var pedidos = await _pedidoService.ListarPorUsuario(usuarioId);
            // return View(pedidos);
            // ─────────────────────────────────────────────────────────

            var pedidos = ObterPedidosFixos();
            return View(pedidos);
        }

        [HttpGet]
        public IActionResult Detalhe(string numero)
        {
            // ─────────────────────────────────────────────────────────
            // var pedido = await _pedidoService.ObterPorNumero(numero);
            // if (pedido == null) return NotFound();
            // return View(pedido);
            // ─────────────────────────────────────────────────────────

            var pedido = ObterPedidosFixos().FirstOrDefault(p => p.NumeroPedido == numero);

            if (pedido == null)
                return RedirectToAction(nameof(MeusPedidos));

            return View(pedido);
        }

        
        private static List<PedidoModel> ObterPedidosFixos()
        {
            return new List<PedidoModel>
    {
        new PedidoModel
        {
            Id            = 1,
            NumeroPedido  = "123456789",
            DataPedido    = DateTime.Now.AddDays(-2),
            Status        = "Pago",
            NomeCompleto  = "João Silva",
            Email         = "joao@email.com",
            Telefone      = "(11) 99999-0001",
            Endereco      = "Rua das Flores",
            Numero        = "100",
            Complemento   = "Apto 12",
            Bairro        = "Centro",
            Cidade        = "São Paulo",
            Estado        = "SP",
            Cep           = "01000-000",
            FormaPagamento = "pix",
            TotalPago     = 3599.10m,
            Itens = new List<PedidoItemModel>
            {
                new PedidoItemModel
                {
                    ProdutoCodigo = 1,
                    ProdutoNome   = "iPhone 13 128GB Meia-Noite",
                    PrecoUnitario = 3999.00m,
                    Quantidade    = 1,
                    Subtotal      = 3599.10m
                }
            }
        },
        new PedidoModel
        {
            Id            = 2,
            NumeroPedido  = "987654321",
            DataPedido    = DateTime.Now.AddDays(-10),
            Status        = "Entregue",
            NomeCompleto  = "João Silva",
            Email         = "joao@email.com",
            Telefone      = "(11) 99999-0001",
            Endereco      = "Rua das Flores",
            Numero        = "100",
            Complemento   = "",
            Bairro        = "Centro",
            Cidade        = "São Paulo",
            Estado        = "SP",
            Cep           = "01000-000",
            FormaPagamento = "credito",
            TotalPago     = 9999.00m,
            Itens = new List<PedidoItemModel>
            {
                new PedidoItemModel
                {
                    ProdutoCodigo = 2,
                    ProdutoNome   = "MacBook Pro M3",
                    PrecoUnitario = 9999.00m,
                    Quantidade    = 1,
                    Subtotal      = 9999.00m
                }
            }
        },
        new PedidoModel
        {
            Id            = 3,
            NumeroPedido  = "456789123",
            DataPedido    = DateTime.Now.AddDays(-1),
            Status        = "Aguardando pagamento",
            NomeCompleto  = "João Silva",
            Email         = "joao@email.com",
            Telefone      = "(11) 99999-0001",
            Endereco      = "Rua das Flores",
            Numero        = "100",
            Complemento   = "",
            Bairro        = "Centro",
            Cidade        = "São Paulo",
            Estado        = "SP",
            Cep           = "01000-000",
            FormaPagamento = "boleto",
            TotalPago     = 299.90m,
            Itens = new List<PedidoItemModel>
            {
                new PedidoItemModel
                {
                    ProdutoCodigo = 3,
                    ProdutoNome   = "Teclado Mecânico Redragon",
                    PrecoUnitario = 299.90m,
                    Quantidade    = 1,
                    Subtotal      = 299.90m
                }
            }
        }
    };
    }
    }
}