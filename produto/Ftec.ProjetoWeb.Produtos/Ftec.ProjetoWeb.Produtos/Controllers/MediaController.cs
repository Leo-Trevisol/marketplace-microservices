using Ftec.ProjetoWeb.Produtos.Aplicacao;
using Ftec.ProjetoWeb.Produtos.Aplicacao.DTO;
using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;
using Ftec.ProjetoWeb.Produtos.Dominio.Enum;
using Ftec.ProjetoWeb.Produtos.Persistencia;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace Ftec.ProjetoWeb.Produtos.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase {

        MediaRepositorio _mediaRepositorio;
        string caminhoUpload;

        public MediaController(IConfiguration config) {
            caminhoUpload = config["uploadPath"];
            _mediaRepositorio = new MediaRepositorio(config["strConexao"], caminhoUpload);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile arquivo) {
            if (arquivo == null || arquivo.Length == 0)
                return BadRequest(new { sucesso = false, message = "Arquivo inválido" });

            var extensao = Path.GetExtension(arquivo.FileName);
            var nomeUnico = Guid.NewGuid() + extensao;

            var caminhoFisico = Path.Combine(caminhoUpload, nomeUnico);
            var caminhoVirtual = $"{caminhoUpload}/{nomeUnico}";

            using (var stream = new FileStream(caminhoFisico, FileMode.Create)) {
                await arquivo.CopyToAsync(stream);
            }

            var media = new Media {
                NomeArquivo = arquivo.FileName,
                NomeUnico = nomeUnico,
                CaminhoArquivo = caminhoVirtual,
                Extensao = extensao,
                TipoArquivo = TipoArquivo.Imagem
            };

            var response = _mediaRepositorio.InserirMedia(media);

            if (response.Sucesso) {
                return Ok(new {
                    sucesso = true,
                    data = new MediaResponse {
                        Id = media.Id,
                        Caminho = media.CaminhoArquivo
                    },
                    message = "Upload realizado com sucesso"
                });
            } else {
                return BadRequest(new {
                    sucesso = false,
                    data = "",
                    message = "Erro ao realizar Upload"
                });
            }


        }

        [HttpGet("obterPorId/{id}")]
        public IActionResult Obter(Guid id) {
            var media = _mediaRepositorio.ObterMedia(id);

            if (media == null) {
                return NotFound(new {
                    sucesso = false,
                    data = (object)null,
                    message = "Mídia não encontrada"
                });
            }

            return Ok(new {
                sucesso = true,
                data = media,
                message = "Mídia encontrada"
            });
        }

        [HttpDelete("deletar/{id}")]
        public IActionResult Deletar(Guid id) {
            var response = _mediaRepositorio.DeletarMedia(id);

            if (!response.Sucesso) {
                return BadRequest(new {
                    sucesso = false,
                    data = false,
                    message = response.Message
                });
            }

            return Ok(new {
                sucesso = true,
                data = true,
                message = "Mídia excluída com sucesso"
            });
        }
    }
}
