using System.ComponentModel;

namespace Ftec.ProjetoWeb.Produtos.Dominio.Enum
{
    public enum TipoArquivo
    {
        [Description("Imagem")]
        Imagem = 0,
        [Description("Arquivo")]
        Arquivo = 1,
        [Description("Vídeo")]
        Video = 2
    }
}
