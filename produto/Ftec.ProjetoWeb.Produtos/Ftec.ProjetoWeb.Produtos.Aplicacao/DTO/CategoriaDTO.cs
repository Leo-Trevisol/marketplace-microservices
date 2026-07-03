namespace Ftec.ProjetoWeb.Produtos.Aplicacao.DTO
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int? Parent_Id { get; set; }
    }
}