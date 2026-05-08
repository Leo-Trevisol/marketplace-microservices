using Ftec.ProjetoWeb.Produtos.Aplicacao;
using Ftec.ProjetoWeb.Produtos.Aplicacao.DTO;
using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace Ftec.ProjetoWeb.Produtos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        ProdutoAplicacao produtoAplicacao;

        public ProdutoController(IConfiguration config) {
            produtoAplicacao = new ProdutoAplicacao(config["strConexao"]);
        }

        [HttpGet]
        public IActionResult ListarProdutos() {
            try {
                var produtos = produtoAplicacao.ListarProdutos();
                return Ok(produtos);
            } catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{texto}")]
        public IActionResult ProcurarPorTexto(string texto) {
            try {
                var produtos = produtoAplicacao.ProcurarPorTexto(texto);
                return Ok(produtos);
            } catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{codigo}")]
        public IActionResult ObtemPorCodigo(string codigo) {
            try {
                var produto = produtoAplicacao.ObterProduto(codigo);
                return Ok(produto);
            } catch (Exception ex) when (ex.Message.Contains("não encontrado")) {
                return NotFound(ex.Message);
            } catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public Response<Produto> InserirProduto([FromBody] ProdutoDTO produto) {
            try {
                var response = produtoAplicacao.AdicionarProduto(produto);
                return response;
            } catch (Exception ex) {
                return new Response<Produto>() {
                    Sucesso = false,
                    Data = null,
                    Message = ex.Message
                };
            }
        }

        [HttpPut]
        public Response<Produto> AtualizarProduto([FromBody] ProdutoDTO produto) {
            try {
                var response = produtoAplicacao.AlterarProduto(produto);
                return response;
            } catch (Exception ex) {
                return new Response<Produto>() {
                    Sucesso = false,
                    Data = null,
                    Message = ex.Message
                };
            }
        }

        [HttpDelete("{codigo}")]
        public Response<Produto> DeleteProduto(string codigo) {
            try {
                var status = produtoAplicacao.ExcluirProduto(codigo);
                var response = new Response<Produto>(status, null, (status ? "Sucesso ao exlcuir produto!" : "Erro ao excluir produto"));
                return response;
            } catch (Exception ex) {
                return new Response<Produto>(false, null, ex.Message);
            }
        }
    }
}
