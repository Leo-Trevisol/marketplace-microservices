using Ftec.ProjetoWeb.Produtos.Aplicacao;
using Ftec.ProjetoWeb.Produtos.Aplicacao.DTO;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult InserirProduto([FromBody] ProdutoDTO produto) {
            try {
                produtoAplicacao.AdicionarProduto(produto);
                return Created(string.Empty, produto);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult AtualizarProduto([FromBody] ProdutoDTO produto) {
            try {
                produtoAplicacao.AlterarProduto(produto);
                return Ok(produto);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{codigo}")]
        public IActionResult DeleteProduto(string codigo) {
            try {
                produtoAplicacao.ExcluirProduto(codigo);
                return NoContent();
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}
