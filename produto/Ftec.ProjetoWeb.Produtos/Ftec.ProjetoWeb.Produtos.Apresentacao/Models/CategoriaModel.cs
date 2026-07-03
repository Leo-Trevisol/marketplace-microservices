namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models
{
    public class CategoriaModel
    {

        public CategoriaModel()
        {

        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int? ParentId { get; set; }
    }

    public class CategoriaArvoreModel {
        public CategoriaArvoreModel() {
            Filhos = new List<CategoriaModel>();
        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public List<CategoriaModel> Filhos { get; set; } = new();
    }
}
