using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;
using Ftec.ProjetoWeb.Produtos.Dominio.Enum;
using Ftec.ProjetoWeb.Produtos.Persistencia;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ftec.ProjetoWeb.Produtos.Controllers
{

    /// <summary>
    /// Controller responsável pelo gerenciamento de mídias e upload de arquivos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {

        MediaRepositorio _mediaRepositorio;
        string caminhoUpload;

        public MediaController(IConfiguration config)
        {
            caminhoUpload = config["uploadPath"];
            _mediaRepositorio = new MediaRepositorio(config["strConexao"], caminhoUpload);
        }

        /// <summary>
        /// Realiza upload de uma imagem.
        /// </summary>
        /// <param name="arquivo">Arquivo enviado via multipart/form-data.</param>
        /// <returns>Dados da mídia salva.</returns>
        [SwaggerOperation(
            Summary = "Upload de imagem",
            Description = "Realiza upload de uma imagem utilizando multipart/form-data."
        )]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile arquivo)
        {

            if (arquivo == null || arquivo.Length == 0)
                return BadRequest(new { sucesso = false, message = "Arquivo inválido" });

            var extensao = Path.GetExtension(arquivo.FileName);
            var nomeUnico = Guid.NewGuid() + extensao;

            var caminhoFisico = Path.Combine(caminhoUpload, nomeUnico);
            var caminhoVirtual = $"{caminhoUpload}/{nomeUnico}";

            using (var stream = new FileStream(caminhoFisico, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            var media = new Media
            {
                NomeArquivo = arquivo.FileName,
                NomeUnico = nomeUnico,
                CaminhoArquivo = caminhoVirtual,
                Extensao = extensao,
                TipoArquivo = TipoArquivo.Imagem
            };

            var response = _mediaRepositorio.InserirMedia(media);

            if (response.Sucesso)
            {
                return Ok(new
                {
                    sucesso = true,
                    data = new MediaResponse
                    {
                        Id = media.Id,
                        Caminho = media.CaminhoArquivo
                    },
                    message = "Upload realizado com sucesso"
                });
            }
            else
            {
                return BadRequest(new
                {
                    sucesso = false,
                    data = "",
                    message = "Erro ao realizar Upload"
                });
            }
        }

        /// <summary>
        /// Obtém uma mídia pelo ID.
        /// </summary>
        /// <param name="id">ID da mídia.</param>
        /// <returns>Dados da mídia encontrada.</returns>
        [SwaggerOperation(
            Summary = "Obter mídia por ID",
            Description = "Retorna os dados de uma mídia através do ID informado."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("obterPorId/{id}")]
        public IActionResult Obter(Guid id)
        {

            var media = _mediaRepositorio.ObterMedia(id);

            if (media == null)
            {
                return NotFound(new
                {
                    sucesso = false,
                    data = (object)null,
                    message = "Mídia não encontrada"
                });
            }

            return Ok(new
            {
                sucesso = true,
                data = media,
                message = "Mídia encontrada"
            });
        }

        /// <summary>
        /// Remove uma mídia pelo ID.
        /// </summary>
        /// <param name="id">ID da mídia.</param>
        /// <returns>Status da exclusão.</returns>
        [SwaggerOperation(
            Summary = "Excluir mídia",
            Description = "Remove uma mídia do sistema através do ID informado."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("deletar/{id}")]
        public IActionResult Deletar(Guid id)
        {

            var response = _mediaRepositorio.DeletarMedia(id);

            if (!response.Sucesso)
            {
                return BadRequest(new
                {
                    sucesso = false,
                    data = false,
                    message = response.Message
                });
            }

            return Ok(new
            {
                sucesso = true,
                data = true,
                message = "Mídia excluída com sucesso"
            });
        }
    }
}