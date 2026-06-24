namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models {
    public class HomeModel{
        public HomeModel() {
            ProdutosDestaque = new List<ProdutoModel>();
            Categorias = new List<CategoriaModel>();
        }
        
        public List<ProdutoModel> ProdutosDestaque { get; set; }
        public List<CategoriaModel> Categorias { get; set; }
    }
}
