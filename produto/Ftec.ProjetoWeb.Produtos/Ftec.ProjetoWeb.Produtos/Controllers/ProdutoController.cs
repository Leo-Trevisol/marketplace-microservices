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

        public ProdutoController(IConfiguration config)
        {
            produtoAplicacao = new ProdutoAplicacao(config["strConexao"]);
        }

        [HttpGet("listar")]
        public Response<List<ProdutoDTO>> ListarProdutos()
        {
            try
            {
                var produtos = produtoAplicacao.ListarProdutos();
                if (produtos != null && produtos.Count > 0)
                {
                    var total = produtos.Count();
                    return new Response<List<ProdutoDTO>>(true, produtos, $"{total} Produtos listados!");
                }
                else
                {
                    return new Response<List<ProdutoDTO>>(false, null, "Nenhum produto cadastrado!");
                }
            }
            catch (Exception ex)
            {
                return new Response<List<ProdutoDTO>>(false, null, $"ERRO! {ex.Message}");
            }
        }

        [HttpGet("buscar/{texto}")]
        public Response<List<ProdutoDTO>> ProcurarPorTexto(string texto)
        {
            try
            {
                var produtos = produtoAplicacao.ProcurarPorTexto(texto);
                if (produtos != null && produtos.Count > 0)
                {
                    var total = produtos.Count();
                    return new Response<List<ProdutoDTO>>(true, produtos, $"{total} Produtos encontrados!");
                }
                else
                {
                    return new Response<List<ProdutoDTO>>(false, null, "Nenhum produto encontrado! Revise o termo de busca");
                }
            }
            catch (Exception ex)
            {
                return new Response<List<ProdutoDTO>>(false, null, $"ERRO! {ex.Message}");
            }
        }

        [HttpGet("obtem/{codigo}")]
        public Response<ProdutoDTO> ObtemPorCodigo(string codigo)
        {
            try
            {
                var produto = produtoAplicacao.ObterProduto(codigo);
                if (produto == null)
                {
                    return new Response<ProdutoDTO>(false, null, "Nenhum produto encontrado. Altere o código ou revise a pesquisa");
                }
                else
                {
                    return new Response<ProdutoDTO>(true, produto, "Produto encontrado");
                }
            }
            catch (Exception ex) when (ex.Message.Contains("não encontrado"))
            {
                return new Response<ProdutoDTO>(false, null, "Produto não encontrado");
            }
            catch (Exception ex)
            {
                return new Response<ProdutoDTO>(false, null, $"ERRO! {ex.Message}");
            }
        }
        [HttpGet("obtemPorId/{id}")]
        public Response<ProdutoDTO> ObtemPorId(string id)
        {
            try
            {
                var produto = produtoAplicacao.ObterProduto(id, true);
                if (produto == null)
                {
                    return new Response<ProdutoDTO>(false, null, "Nenhum produto encontrado. Altere o código ou revise a pesquisa");
                }
                else
                {
                    return new Response<ProdutoDTO>(true, produto, "Produto encontrado");
                }
            }
            catch (Exception ex) when (ex.Message.Contains("não encontrado"))
            {
                return new Response<ProdutoDTO>(false, null, "Produto não encontrado");
            }
            catch (Exception ex)
            {
                return new Response<ProdutoDTO>(false, null, $"ERRO! {ex.Message}");
            }
        }

        [HttpPost("cadastrarProduto")]
        public Response<Produto> InserirProduto([FromBody] ProdutoDTO produto)
        {
            try
            {
                var response = produtoAplicacao.AdicionarProduto(produto);
                return response;
            }
            catch (Exception ex)
            {
                return new Response<Produto>()
                {
                    Sucesso = false,
                    Data = null,
                    Message = ex.Message
                };
            }
        }

        [HttpPut("atualizarProduto")]
        public Response<Produto> AtualizarProduto([FromBody] ProdutoDTO produto)
        {
            try
            {
                var response = produtoAplicacao.AlterarProduto(produto);
                return response;
            }
            catch (Exception ex)
            {
                return new Response<Produto>()
                {
                    Sucesso = false,
                    Data = null,
                    Message = ex.Message
                };
            }
        }

        [HttpDelete("excluirProduto/{id}")]
        public Response<Produto> DeleteProduto(string id)
        {
            try
            {
                Console.WriteLine(id);
                var status = produtoAplicacao.ExcluirProduto(id);
                var response = new Response<Produto>(status, null, (status ? "Sucesso ao excluir produto!" : "Erro ao excluir produto"));
                return response;
            }
            catch (Exception ex)
            {
                return new Response<Produto>(false, null, ex.Message);
            }
        }
    }
}
