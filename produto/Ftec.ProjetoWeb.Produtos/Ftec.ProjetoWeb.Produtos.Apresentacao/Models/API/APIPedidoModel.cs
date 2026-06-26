namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API {
    public class APIPedidoModel {
        public APIPedidoModel() {
            produtosModel = new List<CarrinhoModel>();
        }
        public Guid id { get; set; }
        public Guid usuarioId { get; set; }
        public List<CarrinhoModel> produtosModel { get; set; }
        public DateTime dataPedido { get; set; }
        public int statusPedido { get; set; }
        public decimal valorTotal { get; set; }
        public string cepEnderecoEntrega { get; set; }
        public string numeroEnderecoEntrega { get; set; }
    }
}