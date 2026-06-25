namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API
{
    public class ApiErroResponse
    {
        public List<ApiErroDetalhe>? Erros { get; set; }
    }

    public class ApiErroDetalhe
    {
        public string? Campo { get; set; }
        public string? Mensagem { get; set; }
    }
}