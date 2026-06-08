namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models {
    public class CategoriaModel {

        public CategoriaModel() {

        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int? ParentId { get; set; }
    }
}
