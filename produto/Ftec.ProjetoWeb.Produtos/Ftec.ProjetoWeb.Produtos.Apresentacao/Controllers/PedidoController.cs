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
    }
}