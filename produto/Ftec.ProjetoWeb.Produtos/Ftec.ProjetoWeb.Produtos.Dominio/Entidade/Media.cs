using Ftec.ProjetoWeb.Produtos.Dominio.Enum;

namespace Ftec.ProjetoWeb.Produtos.Dominio.Entidade
{
    public class Media : GenericEntity
    {
        public Media()
        {
            Id = Guid.NewGuid();
            DataUpload = DateTime.UtcNow;
        }

        public string NomeArquivo { get; set; }
        public string NomeUnico { get; set; }
        public string CaminhoArquivo { get; set; }
        public TipoArquivo TipoArquivo { get; set; }
        public string Extensao { get; set; }
        public DateTime DataUpload { get; set; }

    }
    public class MediaResponse
    {
        public Guid Id { get; set; }
        public string Caminho { get; set; }

    }

}
