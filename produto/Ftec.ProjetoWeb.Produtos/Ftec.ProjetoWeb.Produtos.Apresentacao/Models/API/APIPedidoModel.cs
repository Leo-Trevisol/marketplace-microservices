namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API {
    public class APIPedidoModel {
        public APIPedidoModel() {
            produtosModel = new List<APIPedidoIntemModel>();
        }
        public Guid id { get; set; }
        public Guid usuarioId { get; set; }
        public List<APIPedidoIntemModel> produtosModel { get; set; }
        public DateTime dataPedido { get; set; }
        public int statusPedido { get; set; }
        public decimal valorTotal { get; set; }
        public string cepEnderecoEntrega { get; set; }
        public string numeroEnderecoEntrega { get; set; }
    }
    public class APIPedidoIntemModel{
        public APIPedidoIntemModel() { 
        }
        public Guid id { get; set; }
        public Guid pedidoId { get; set; }
        public Guid produtoId { get; set; }
        public decimal preco { get; set; }
        public int quantidade { get; set; }
        public bool disponivel { get; set; }
        public bool excluido { get; set; }

    }
}