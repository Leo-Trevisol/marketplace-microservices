using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ftec.ProjetoWeb.Produtos.Dominio.Interfaces
{
    public interface IProdutoRepositorio
    {
        void CriarProduto(Produto produto);
        void AlterarProduto(Produto produto);
        void ExcluirProduto(string codigo);
        Produto ObtemPorCodigo(string codigo);
        List<Produto> ProcurarPorTexto(string texto);
        List<Produto> ListaTodosProdutos();
    }
}
