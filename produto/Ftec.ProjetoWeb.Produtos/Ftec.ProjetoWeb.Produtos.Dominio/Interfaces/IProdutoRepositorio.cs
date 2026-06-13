using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;

namespace Ftec.ProjetoWeb.Produtos.Dominio.Interfaces
{
    public interface IProdutoRepositorio
    {
        Response<Produto> CriarProduto(Produto produto);
        Response<Produto> AlterarProduto(Produto produto);
        bool ExcluirProduto(string codigo);
        Produto ObtemPorCodigo(string codigo);
        Produto ObtemPorId(string id);
        List<Produto> ProcurarPorTexto(string texto);
        List<Produto> ListaTodosProdutos();
    }
}
