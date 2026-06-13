namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models
{
    public class ProdutoModel
    {

        public ProdutoModel()
        {

        }
        public Guid Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
        public int EstoqueMinimoVenda { get; set; }
        public int? IdCategoria { get; set; }
        public Guid IdImagemPrincipal { get; set; }
        public string Descricao { get; set; }
        public bool Destaque { get; set; }
        public bool Disponivel { get; set; }
        public bool Excluido { get; set; }

        #region Model Members
        public CategoriaModel Categoria { get; set; }
        public MediaModel ImagemPrincipal { get; set; }
        #endregion

    }
}
