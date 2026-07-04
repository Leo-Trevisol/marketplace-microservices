using Ftec.ProjetoWeb.Produtos.Apresentacao.Facade;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
public class MediaController : Controller
{
    private readonly APIFacade _apiFacade;

    public MediaController(IConfiguration configuration)
    {
        _apiFacade = new APIFacade(configuration);
    }

    [HttpPost("Upload")]
    public async Task<IActionResult> Upload(IFormFile arquivo)
    {
        if (arquivo == null || arquivo.Length == 0)
            return BadRequest(new
            {
                sucesso = false,
                message = "Arquivo inválido."
            });

        var resultado = await _apiFacade.UploadImagemAsync(arquivo);

        return Json(resultado);
    }
}