using Ftec.ProjetoWeb.Produtos.Apresentacao.Enums;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models
{
    public class MediaModel
    {
        public Guid Id { get; set; }

        public string NomeArquivo { get; set; }
        public string NomeUnico { get; set; }
        public string CaminhoArquivo { get; set; }
        public TipoArquivo TipoArquivo { get; set; }
        public string Extensao { get; set; }
        public DateTime DataUpload { get; set; }
    }
}
