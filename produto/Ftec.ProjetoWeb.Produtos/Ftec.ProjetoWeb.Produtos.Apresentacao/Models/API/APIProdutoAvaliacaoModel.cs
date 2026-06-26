namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API {
    public class APIProdutoAvaliacaoModel {
        public APIProdutoAvaliacaoModel() {

        }
        public Guid Id { get; set; }
        public Guid idCliente { get; set; }
        public Guid idProduto { get; set; }
        public int Nota { get; set; }
        public string Descricao { get; set; }
    }
}
