using Ftec.ProjetoWeb.Produtos.Apresentacao.Facade;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Controllers
{
    public class PedidoController : Controller
    {
        private readonly CarrinhoAPIService _carrinhoService;
        private readonly APIFacade _apiFacade;
        private readonly IConfiguration _config;

        public PedidoController(IConfiguration config, CarrinhoAPIService carrinhoService)
        {
            _config = config;
            _apiFacade = new APIFacade(config);
            _carrinhoService = carrinhoService;
        }

        [HttpPost]
        public IActionResult RegistrarPedido(Guid usuarioId, string cepEntrega, string numeroEntrega)
        {
            var carrinho = _carrinhoService.ObterCarrinho();

            if (!carrinho.Any())
                return RedirectToAction("Index", "Carrinho");

            if (string.IsNullOrWhiteSpace(cepEntrega) || string.IsNullOrWhiteSpace(numeroEntrega))
            {
                TempData["Erro"] = "Informe o CEP e o número de entrega para continuar.";
                return RedirectToAction("Index", "Carrinho");
            }

            try
            {
                var pedido = new APIPedidoRegistrarModel
                {
                    id = Guid.NewGuid(),
                    usuarioId = usuarioId,
                    produtosModel = carrinho.Select(item => new APIPedidoProdutoModel
                    {
                        produtoId = Guid.Parse(item.IdProduto.ToString()),
                        quantidade = item.Quantidade,
                    }).ToList(),
                    cepEnderecoEntrega = cepEntrega,
                    numeroEnderecoEntrega = numeroEntrega
                };

                var idPedidoCriado = _apiFacade.AdicionarPedidoRetornando(pedido);

                if (!idPedidoCriado.HasValue)
                {
                    TempData["Erro"] = "Não foi possível registrar o pedido. Tente novamente.";
                    return RedirectToAction("Index", "Carrinho");
                }

                var cepOrigem = _config["CepOrigemLoja"] ?? "90000-000";

                var transportadoraId = _apiFacade.ObterTransportadoraPadrao();

                if (!transportadoraId.HasValue)
                {
                    TempData["Erro"] = "Nenhuma transportadora disponível no momento.";
                    return RedirectToAction("Index", "Carrinho");
                }

                var enderecoEntregaId = Guid.NewGuid();

                Console.WriteLine($"[FRETE] Calculando para pedido: {idPedidoCriado.Value}, origem: {cepOrigem}, destino: {cepEntrega}, transportadora: {transportadoraId}, endereco: {enderecoEntregaId}");

                var frete = _apiFacade.CalcularFrete(idPedidoCriado.Value, cepOrigem, cepEntrega, transportadoraId.Value, enderecoEntregaId);

                if (frete == null)
                {
                    Console.WriteLine("[FRETE] Resultado: NULL (deu erro)");
                    TempData["Erro"] = "Não foi possível calcular o frete para este endereço. Tente novamente.";
                    return RedirectToAction("Index", "Carrinho");
                }

                Console.WriteLine($"[FRETE] Resultado: R$ {frete.valorFrete}");

                TempData["ValorFrete"] = frete.valorFrete.ToString(System.Globalization.CultureInfo.InvariantCulture);
                TempData["FreteId"] = frete.idFrete.ToString();

                return RedirectToAction("Index", "Pagamento", new { idPedido = idPedidoCriado.Value });
            }
            catch (Exception ex)
            {
                return Content($"ERRO: {ex.Message}");
            }
        }

        public IActionResult MeusPedidos()
        {
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");

            if (string.IsNullOrEmpty(usuarioIdStr) || !Guid.TryParse(usuarioIdStr, out var usuarioId))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Guid user = new Guid("85072fea-1ca3-4be0-9e37-d3a259bf47db");
            //var pedidosApi = _apiFacade.ListarPedidosPorUsuario(user);

            var pedidosApi = _apiFacade.ListarPedidosPorUsuario(usuarioId);

            var pedidos = pedidosApi.Select(MapearPedidoModel).ToList();

            return View(pedidos);
        }

        public IActionResult Detalhe(string numero)
        {
            if (!Guid.TryParse(numero, out var pedidoId))
                return NotFound();


            var response = _apiFacade.ObterPedidoPorId(pedidoId);

            var pedido = MapearPedidoModel(response);

            return View(pedido);
        }

        private PedidoModel MapearPedidoModel(APIPedidoModel apiPedido)
        {
            var itens = apiPedido.produtosModel.Select(item =>
            {
                var produto = _apiFacade.ObterProduto(item.produtoId.ToString());



                return new PedidoItemModel
                {
                    ProdutoCodigo = 0,
                    ProdutoNome = produto?.Nome ?? "Produto não encontrado",
                    PrecoUnitario = item.preco,
                    Quantidade = item.quantidade,
                    Subtotal = item.preco * item.quantidade,
                    ImagemPrincipal = produto?.ImagemPrincipal.NomeUnico
                };
            }).ToList();

            var frete = _apiFacade.ObterFretePorPedido(apiPedido.id);

            var model = new PedidoModel
            {
                NumeroPedido = apiPedido.id.ToString(),
                DataPedido = apiPedido.dataPedido,
                Status = MapearStatusPedido(apiPedido.statusPedido),
                TotalPago = apiPedido.valorTotal,
                Itens = itens,

                Cep = frete?.cepDestino ?? apiPedido.cepEnderecoEntrega,
                Numero = frete?.numero ?? apiPedido.numeroEnderecoEntrega,
                Endereco = frete?.logradouro ?? "",
                Complemento = frete?.complemento ?? "",
                Bairro = frete?.bairro ?? "",
                Cidade = frete?.cidade ?? "",
                Estado = frete?.estado ?? "",

                FormaPagamento = "credito",

                NomeCompleto = HttpContext.Session.GetString("Nome") ?? "",
            };

            return model;
        }

        private string MapearStatusPedido(int status) => status switch
        {
            0 => "Aguardando pagamento",
            1 => "Pago",
            2 => "Enviado",
            3 => "Entregue",
            4 => "Cancelado",
            _ => "Desconhecido"
        };
    }
}
