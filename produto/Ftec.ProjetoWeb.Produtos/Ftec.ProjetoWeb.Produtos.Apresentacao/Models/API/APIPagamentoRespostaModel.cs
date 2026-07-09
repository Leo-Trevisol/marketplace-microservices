namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API
{
    public class APIPagamentoRespostaModel
    {
        public Guid pagamentoId { get; set; }
        public decimal valorTotal { get; set; }
        public string statusPagamento { get; set; }
    }
}