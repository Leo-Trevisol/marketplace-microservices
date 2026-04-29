using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ftec.ProjetoWeb.Produtos.Dominio.Interfaces
{
    public interface IProdutoRepositorio
    {
        void Inserir(Produto produto);
        void Alterar(Produto produto);
        void Excluir(string codigo);
        Produto Procurar(string codigo);
        List<Produto> ProcurarTodos();
    }
}
