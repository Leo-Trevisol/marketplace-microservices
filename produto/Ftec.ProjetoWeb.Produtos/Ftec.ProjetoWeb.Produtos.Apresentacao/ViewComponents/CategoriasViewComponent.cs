using Ftec.ProjetoWeb.Produtos.Apresentacao.Facade;
using Microsoft.AspNetCore.Mvc;

public class CategoriasViewComponent : ViewComponent {
    private readonly APIFacade _apiFacade;
    // ou o repositório/service que você já usa na HomeController

    public CategoriasViewComponent(IConfiguration config) {
        _apiFacade = new APIFacade(config);
    }

    public async Task<IViewComponentResult> InvokeAsync() {
        var categorias = _apiFacade.ListarCategorias();
        return View(categorias); 
    }
}