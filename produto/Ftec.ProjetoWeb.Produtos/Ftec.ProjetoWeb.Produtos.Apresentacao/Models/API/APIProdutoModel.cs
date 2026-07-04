using System.ComponentModel.DataAnnotations;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API
{
    public class APIProdutoModel
    {
        public APIProdutoModel()
        {

        }
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Código obrigatório")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Nome obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preço obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "Quantidade em estoque obrigatória")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantidade inválida")]
        public int QuantidadeEstoque { get; set; }

        [Required(ErrorMessage = "Estoque mínimo obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = "Estoque mínimo inválido")]
        public int EstoqueMinimoVenda { get; set; }

        [Required(ErrorMessage = "Categoria obrigatória")]
        public int IdCategoria { get; set; }
        public Guid IdImagemPrincipal { get; set; }

        [Required(ErrorMessage = "Descrição obrigatória")]
        public string Descricao { get; set; }

        public bool Disponivel { get; set; }
        public bool Destaque { get; set; }

    }
}