using System.ComponentModel.DataAnnotations;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models
{
    public class PagamentoModel
    {
        public int ProdutoCodigo { get; set; }
        public string ProdutoNome { get; set; }
        public decimal ProdutoPreco { get; set; }

        [Required(ErrorMessage = "Nome obrigatório")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "E-mail obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "CPF obrigatório")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Telefone obrigatório")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "CEP obrigatório")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Endereço obrigatório")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Número obrigatório")]
        public string Numero { get; set; }

        public string? Complemento { get; set; }

        [Required(ErrorMessage = "Bairro obrigatório")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Cidade obrigatória")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Estado obrigatório")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "Forma de pagamento obrigatória")]
        public string FormaPagamento { get; set; }

        public string? NumeroCartao { get; set; }
        public string? NomeTitular { get; set; }
        public string? Validade { get; set; }
        public string? Cvv { get; set; }
        public int Parcelas { get; set; } = 1;
    }
}