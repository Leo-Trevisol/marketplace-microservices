namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models
{
    public class PedidoItemModel
    {
        public int ProdutoCodigo { get; set; }
        public string ProdutoNome { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public decimal Subtotal { get; set; }

        public string? ImagemPrincipal { get; set; }
    }
}