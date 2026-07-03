namespace Ftec.ProjetoWeb.Produtos.Dominio.Entidade
{
    public class Categoria
    {
        public Categoria()
        {
            Id = 0;
            Nome = string.Empty;
            Descricao = string.Empty;
            Parent_Id = 0;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int? Parent_Id { get; set; }
    }
}
