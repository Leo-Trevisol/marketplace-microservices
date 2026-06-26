namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models {
    public class CarrinhoModel {
        public CarrinhoModel (){

        }
        public Guid IdProduto { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public decimal Preco { get; set; }
        public decimal Subtotal { get; set; }
        public int Quantidade { get; set; }
    }
}
