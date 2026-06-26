using System.ComponentModel.DataAnnotations;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API
{
    public class APICategoriaModel
    {
        public APICategoriaModel()
        {
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Nome obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres")]
        public string Nome { get; set; }

        // Na tabela está como TEXT NULL, então não é obrigatório
        public string Descricao { get; set; }

        // Na tabela está como INT4 NULL (pode ser nulo se for uma categoria raiz)
        public int? ParentId { get; set; }
    }
}