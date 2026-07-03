using Ftec.ProjetoWeb.Produtos.Apresentacao.Facade;
using Microsoft.AspNetCore.Mvc;

public class CategoriasMobileViewComponent : ViewComponent
{
    private readonly APIFacade _apiFacade;

    public CategoriasMobileViewComponent(IConfiguration configuration)
    {
        _apiFacade = new APIFacade(configuration);
    }

    public IViewComponentResult Invoke()
    {
        var categorias = _apiFacade.ListarCategorias();

        return View(categorias);
    }
}