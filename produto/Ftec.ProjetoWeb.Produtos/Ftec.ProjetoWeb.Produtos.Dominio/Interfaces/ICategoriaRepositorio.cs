using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;

namespace Ftec.ProjetoWeb.Produtos.Dominio.Interfaces
{
    public interface ICategoriaRepositorio
    {
        Response<Categoria> CriarCategoria(Categoria categoria);
        Response<Categoria> AlterarCategoria(Categoria categoria);
        bool ExcluirCategoria(int id);
        Categoria ObterPorId(int id);
        List<Categoria> ObterTodos();
        List<Categoria> ObterPorTexto(string texto);


    }
}
