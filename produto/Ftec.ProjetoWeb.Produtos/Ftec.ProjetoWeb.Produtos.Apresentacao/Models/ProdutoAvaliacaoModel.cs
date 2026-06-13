namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models
{
    public class ProdutoAvaliacaoModel
    {
        public ProdutoAvaliacaoModel()
        {

        }
        public Guid Id { get; set; }
        public Guid idCliente { get; set; }
        public Guid idProduto { get; set; }
        public int Nota { get; set; }
        public string Descricao { get; set; }
    }
}
