using Ftec.ProjetoWeb.Produtos.Aplicacao;
using Ftec.ProjetoWeb.Produtos.Aplicacao.DTO;
using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ftec.ProjetoWeb.Produtos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        CategoriaAplicacao categoriaAplicacao;

        public CategoriaController(IConfiguration config)
        {
            categoriaAplicacao = new CategoriaAplicacao(config["strCategoriaConexao"]);
        }

        [HttpGet("arvore")]
        public Response<List<CategoriaArvoreDTO>> ListarArvoreCategorias()
        {
            try
            {
                var categorias = categoriaAplicacao.ObterArvoreCategorias();
                if (categorias != null && categorias.Count > 0)
                {

                    return new Response<List<CategoriaArvoreDTO>>
                    {
                        Sucesso = true,
                        Data = categorias,
                        Message = "Categorias listadas com sucesso."
                    };
                }
                else
                {
                    return new Response<List<CategoriaArvoreDTO>>
                    {
                        Sucesso = false,
                        Data = null,
                        Message = $"Nenhuma categoria cadastrada"
                    };
                }
            }
            catch (Exception ex)
            {
                return new Response<List<CategoriaArvoreDTO>>
                {
                    Sucesso = false,
                    Data = null,
                    Message = $"Erro ao listar árvore de categorias: {ex.Message}"
                };
            }
        }

        [HttpGet("listar")]
        public Response<List<CategoriaDTO>> ListarCategorias()
        {
            try
            {
                var categorias = categoriaAplicacao.ObterCategorias();
                if (categorias != null && categorias.Count > 0)
                {

                    return new Response<List<CategoriaDTO>>
                    {
                        Sucesso = true,
                        Data = categorias,
                        Message = "Categorias listadas com sucesso."
                    };
                }
                else
                {
                    return new Response<List<CategoriaDTO>>
                    {
                        Sucesso = false,
                        Data = null,
                        Message = $"Nenhuma categoria cadastrada"
                    };
                }

            }
            catch (Exception ex)
            {
                return new Response<List<CategoriaDTO>>
                {
                    Sucesso = false,
                    Data = null,
                    Message = $"Erro ao listar categorias: {ex.Message}"
                };
            }
        }

        [HttpGet("listar/{texto}")]
        public Response<List<CategoriaDTO>> ListarCategoriasPorTexto(string texto)
        {
            try
            {
                var categorias = categoriaAplicacao.ObterCategoriasPorTexto(texto);
                if (categorias != null && categorias.Count > 0)
                {
                    return new Response<List<CategoriaDTO>>
                    {
                        Sucesso = true,
                        Data = categorias,
                        Message = "Categorias listadas com sucesso."
                    };
                }
                else
                {
                    return new Response<List<CategoriaDTO>>
                    {
                        Sucesso = false,
                        Data = null,
                        Message = $"Nenhuma categoria encontrada para o texto: {texto}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new Response<List<CategoriaDTO>>
                {
                    Sucesso = false,
                    Data = null,
                    Message = $"Erro ao listar categorias: {ex.Message}"
                };
            }
        }

        [HttpGet("obterPorId/{id}")]
        public Response<CategoriaDTO> ObterPorId(int id)
        {
            try
            {
                var categoria = categoriaAplicacao.ObterCategoria(id);
                if (categoria == null)
                {
                    return new Response<CategoriaDTO>
                    {
                        Sucesso = false,
                        Data = null,
                        Message = $"Categoria não encontrada"
                    };
                }
                else
                {
                    return new Response<CategoriaDTO>
                    {
                        Sucesso = true,
                        Data = categoria,
                        Message = $"Categoria encontrada"
                    };
                }
            }
            catch (Exception ex) when (ex.Message.Contains("não encontrada"))
            {
                return new Response<CategoriaDTO>(false, null, "Categoria não encontrada");
            }
            catch (Exception ex)
            {
                return new Response<CategoriaDTO>(false, null, $"ERRO! {ex.Message}");
            }
        }

        [HttpPost("cadastrar")]
        public Response<Categoria> CadastrarCategoria([FromBody] CategoriaDTO categoria)
        {
            try
            {
                var response = categoriaAplicacao.AdicionarCategoria(categoria);
                return response;
            }
            catch (Exception ex)
            {
                return new Response<Categoria>()
                {
                    Sucesso = false,
                    Data = null,
                    Message = ex.Message
                };
            }
        }

        [HttpPut("alterar")]
        public Response<Categoria> AlterarCategoria([FromBody] CategoriaDTO categoria)
        {
            try
            {
                var response = categoriaAplicacao.AlterarCategoria(categoria);
                return response;
            }
            catch (Exception ex)
            {
                return new Response<Categoria>()
                {
                    Sucesso = false,
                    Data = null,
                    Message = ex.Message
                };
            }
        }

        [HttpDelete("excluir/{id}")]
        public Response<Categoria> ExcluirCategoria(int id)
        {
            try
            {
                var status = categoriaAplicacao.ExcluirCategoria(id);
                var response = new Response<Categoria>(status, null, (status ? "Sucesso ao excluir categoria!" : "Erro ao excluir categoria"));
                return response;
            }
            catch (Exception ex)
            {
                return new Response<Categoria>()
                {
                    Sucesso = false,
                    Data = null,
                    Message = ex.Message
                };
            }
        }
    }
}
