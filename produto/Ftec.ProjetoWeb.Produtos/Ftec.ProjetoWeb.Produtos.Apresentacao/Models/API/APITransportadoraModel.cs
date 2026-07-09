namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API
{
    public class APITransportadoraModel
    {
        public Guid transportadoraId { get; set; }
        public string nome { get; set; } = string.Empty;
        public string codigoServico { get; set; } = string.Empty;
        public decimal valorBase { get; set; }
        public int prazoMinDias { get; set; }
        public int prazoMaxDias { get; set; }
        public bool ativo { get; set; }

        // Propriedades auxiliares para exibição
        public string PrazoFormatado => $"{prazoMinDias}-{prazoMaxDias} dias";
        public string ValorFormatado => valorBase.ToString("C");
    }
}