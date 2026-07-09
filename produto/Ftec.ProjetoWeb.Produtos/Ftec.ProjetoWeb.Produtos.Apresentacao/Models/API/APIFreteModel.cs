namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API
{
    public class APIFreteModel
    {
        public Guid idFrete { get; set; }
        public Guid pedidoId { get; set; }
        public Guid transportadoraId { get; set; }
        public Guid enderecoEntregaId { get; set; }

        public decimal valorFrete { get; set; }
        public string statusEntrega { get; set; } = "Pendente";

        public string cepDestino { get; set; } = string.Empty;
        public string logradouro { get; set; } = string.Empty;
        public string numero { get; set; } = string.Empty;
        public string complemento { get; set; } = string.Empty;
        public string bairro { get; set; } = string.Empty;
        public string cidade { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;

        public string codigoRastreio { get; set; } = string.Empty;
        public string nomeTransportadora { get; set; } = string.Empty;

        public DateTime criadoEm { get; set; }
        public DateTime? dataEnvio { get; set; }
        public DateTime? dataEntrega { get; set; }

        public int prazoEntrega { get; set; }

        // Propriedades auxiliares para exibição
        public string EnderecoCompleto =>
            $"{logradouro}, {numero}{(string.IsNullOrEmpty(complemento) ? "" : " - " + complemento)}, {bairro}, {cidade} - {estado}, CEP {cepDestino}";

        public bool EstaPendente => statusEntrega?.ToLower() == "pendente";
        public bool EstaEmTransito => statusEntrega?.ToLower() == "emtransito";
        public bool EstaEntregue => statusEntrega?.ToLower() == "entregue";

        public string StatusBadgeClass => statusEntrega?.ToLower() switch
        {
            "pendente" => "bg-warning text-dark",
            "preparando" => "bg-info text-white",
            "enviado" => "bg-primary text-white",
            "emtransito" => "bg-secondary text-white",
            "entregue" => "bg-success text-white",
            _ => "bg-secondary text-white"
        };

        public string StatusIcone => statusEntrega?.ToLower() switch
        {
            "pendente" => "bi-clock",
            "preparando" => "bi-hourglass-split",
            "enviado" => "bi-truck",
            "emtransito" => "bi-arrow-right-circle",
            "entregue" => "bi-box-seam",
            _ => "bi-question-circle"
        };
    }

    public class APIFreteCalcularModel
    {
        public Guid pedidoId { get; set; }
        public string cepOrigem { get; set; } = string.Empty;
        public string cepDestino { get; set; } = string.Empty;
    }

    public class APIFreteEnvioModel
    {
        public string codigoRastreio { get; set; } = string.Empty;
    }
}