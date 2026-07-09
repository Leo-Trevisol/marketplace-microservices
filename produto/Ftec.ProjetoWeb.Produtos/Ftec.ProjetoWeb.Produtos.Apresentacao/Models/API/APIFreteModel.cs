namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API
{
    public class APIFreteModel
    {
        public Guid idFrete { get; set; }
        public Guid pedidoId { get; set; }
        public Guid transportadoraId { get; set; }
        public Guid enderecoEntregaId { get; set; }

        public decimal valorFrete { get; set; }
        public int statusEntrega { get; set; }

        public string StatusEntregaTexto => statusEntrega switch
        {
            0 => "Pendente",
            1 => "Preparando",
            2 => "Enviado",
            3 => "EmTransito",
            4 => "Entregue",
            _ => "Desconhecido"
        };

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

        public bool EstaPendente => statusEntrega == 0;
        public bool EstaEmTransito => statusEntrega == 3;
        public bool EstaEntregue => statusEntrega == 4;

        public string StatusBadgeClass => statusEntrega switch
        {
            0 => "bg-warning text-dark",   // Pendente
            1 => "bg-info text-white",     // Preparando
            2 => "bg-primary text-white",  // Enviado
            3 => "bg-secondary text-white",// EmTransito
            4 => "bg-success text-white",  // Entregue
            _ => "bg-secondary text-white"
        };

        public string StatusIcone => statusEntrega switch
        {
            0 => "bi-clock",
            1 => "bi-hourglass-split",
            2 => "bi-truck",
            3 => "bi-arrow-right-circle",
            4 => "bi-box-seam",
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