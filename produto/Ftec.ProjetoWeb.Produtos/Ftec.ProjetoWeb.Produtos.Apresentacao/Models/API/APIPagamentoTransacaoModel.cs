namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API
{
    public class APIPagamentoTransacaoModel
    {
        public Guid pagamentoId { get; set; }
        public decimal valor { get; set; }
        public string retornoGateway { get; set; }
        public bool statusTransacao { get; set; }
    }
}