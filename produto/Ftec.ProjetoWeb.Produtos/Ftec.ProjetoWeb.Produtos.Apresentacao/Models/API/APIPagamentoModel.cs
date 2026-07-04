namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API
{
    public class APIPagamentoModel
    {
        // Campos enviados na criação
        public Guid pedidoId { get; set; }
        public string cpfCliente { get; set; }
        public decimal valorTotal { get; set; }
        public int metodoPagamento { get; set; }

        // Campos que voltam na resposta da API
        public Guid pagamentoId { get; set; }
        public string statusPagamento { get; set; }
    }
}