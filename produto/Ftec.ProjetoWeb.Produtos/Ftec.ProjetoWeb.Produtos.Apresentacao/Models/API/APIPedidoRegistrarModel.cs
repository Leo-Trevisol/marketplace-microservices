namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API {
    public class APIPedidoRegistrarModel {
        public APIPedidoRegistrarModel() {
            produtosModel = new List<APIPedidoProdutoModel>();
        }
        public Guid id { get; set; }
        public Guid usuarioId { get; set; }
        public List<APIPedidoProdutoModel> produtosModel { get; set; }
        public string cepEnderecoEntrega { get; set; }
        public string numeroEnderecoEntrega { get; set; }
    }
    public class APIPedidoProdutoModel {
        public Guid produtoId { get; set; }
        public int quantidade { get; set; }
    }
}
