namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models
{
    public class PedidoModel {
         
        public int Id { get; set; }
        public string NumeroPedido { get; set; }
        public DateTime DataPedido { get; set; }
        public string Status { get; set; }

        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }

        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }

        public string FormaPagamento { get; set; }
        public decimal TotalPago { get; set; }

        public List<PedidoItemModel> Itens { get; set; } = new();

        // Propriedades auxiliares para a view
        public string EnderecoCompleto =>
            $"{Endereco}, {Numero}{(string.IsNullOrEmpty(Complemento) ? "" : " - " + Complemento)}, {Bairro}, {Cidade} - {Estado}, CEP {Cep}";

        public string FormaPagamentoLabel => FormaPagamento switch {
            "pix" => "PIX",
            "credito" => "Cartão de Crédito",
            "boleto" => "Boleto Bancário",
            _ => FormaPagamento
        };

        public string IconePagamento => FormaPagamento switch {
            "pix" => "bi-qr-code",
            "credito" => "bi-credit-card",
            "boleto" => "bi-upc",
            _ => "bi-cash"
        };

        public string StatusBadgeClass => Status switch {
            "Aguardando pagamento" => "bg-warning text-dark",
            "Pago" => "bg-success",
            "Enviado" => "bg-primary",
            "Entregue" => "bg-secondary",
            "Cancelado" => "bg-danger",
            _ => "bg-secondary"
        };

        public string StatusIcone => Status switch {
            "Aguardando pagamento" => "bi-clock",
            "Pago" => "bi-check-circle",
            "Enviado" => "bi-truck",
            "Entregue" => "bi-box-seam",
            "Cancelado" => "bi-x-circle",
            _ => "bi-circle"
        };
    }
}