using Ftec.ProjetoWeb.Produtos.Aplicacao;
using Ftec.ProjetoWeb.Produtos.Aplicacao.DTO;
using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ftec.ProjetoWeb.Produtos.Controllers
{
    /// <summary>
    /// Controller responsável pelo gerenciamento de produtos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        ProdutoAplicacao produtoAplicacao;

        public ProdutoController(IConfiguration config)
        {
            produtoAplicacao = new ProdutoAplicacao(config["strConexao"]);
        }

        /// <summary>
        /// Lista todos os produtos cadastrados.
        /// </summary>
        /// <returns>Lista de produtos.</returns>

        [SwaggerOperation(
            Summary = "Listar produtos",
            Description = "Retorna todos os produtos cadastrados no sistema."
        )]
        [ProducesResponseType(typeof(Response<List<ProdutoDTO>>), StatusCodes.Status200OK)]
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

        /// <summary>
        /// Busca produtos por nome, código ou descrição.
        /// </summary>
        /// <param name="texto">Texto utilizado na pesquisa.</param>
        /// <returns>Produtos encontrados.</returns>
        [SwaggerOperation(
            Summary = "Buscar produtos",
            Description = "Realiza busca textual de produtos."
        )]
        [ProducesResponseType(typeof(Response<List<ProdutoDTO>>), StatusCodes.Status200OK)]
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

        /// <summary>
        /// Obtém um produto pelo código.
        /// </summary>
        /// <param name="codigo">Código do produto.</param>
        /// <returns>Produto encontrado.</returns>
        [SwaggerOperation(
            Summary = "Obter produto por código",
            Description = "Retorna um produto através do código informado."
        )]
        [ProducesResponseType(typeof(Response<ProdutoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Obtém um produto pelo ID.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <returns>Produto encontrado.</returns>
        [SwaggerOperation(
            Summary = "Obter produto por ID",
            Description = "Retorna um produto através do ID informado."
        )]
        [ProducesResponseType(typeof(Response<ProdutoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Cadastra um novo produto.
        /// </summary>
        /// <param name="produto">Dados do produto.</param>
        /// <returns>Produto cadastrado.</returns>
        [SwaggerOperation(
            Summary = "Cadastrar produto",
            Description = "Realiza o cadastro de um novo produto."
        )]
        [ProducesResponseType(typeof(Response<Produto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        /// <param name="produto">Dados atualizados do produto.</param>
        /// <returns>Produto atualizado.</returns>
        [SwaggerOperation(
            Summary = "Atualizar produto",
            Description = "Atualiza os dados de um produto existente."
        )]
        [ProducesResponseType(typeof(Response<Produto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Remove um produto.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <returns>Status da exclusão.</returns>
        [SwaggerOperation(
            Summary = "Excluir produto",
            Description = "Remove um produto do sistema."
        )]
        [ProducesResponseType(typeof(Response<Produto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
