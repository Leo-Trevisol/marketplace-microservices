namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API
{
    public class APIPagamentoModel
    {
        public Guid pedidoId { get; set; }
        public string cpfCliente { get; set; }
        public decimal valorTotal { get; set; }
        public int metodoPagamento { get; set; }
    }
}