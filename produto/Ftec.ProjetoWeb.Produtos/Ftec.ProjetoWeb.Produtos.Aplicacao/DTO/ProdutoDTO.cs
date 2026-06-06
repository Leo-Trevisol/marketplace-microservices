using System;
using System.Collections.Generic;
using System.Text;

namespace Ftec.ProjetoWeb.Produtos.Aplicacao.DTO
{
    public class ProdutoDTO
    {
        public ProdutoDTO()
        {

        }
        public Guid Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
        public int EstoqueMinimoVenda { get; set; }
        public int IdCategoria { get; set; }
        public Guid IdImagemPrincipal { get; set; }
        //public MediaDTO Media { get; set; }
        public string Descricao { get; set; }
        public bool Destaque { get; set; }
        public bool Disponivel { get; set; }
        public bool Excluido { get; set; }
    }
}
