namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models
{
    public class CategoriaListaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int? ParentId { get; set; }
        public int Nivel { get; set; } // 0 para pai, 1 para filho, 2 para neto...
        public string CategoriaPaiNome { get; set; } // Opcional: para exibir o nome do pai direto
    }
}
