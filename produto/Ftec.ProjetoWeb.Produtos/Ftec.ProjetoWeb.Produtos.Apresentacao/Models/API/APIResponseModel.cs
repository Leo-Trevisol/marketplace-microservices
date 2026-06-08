namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API {
    public class APIResponseModel<T> {
        public bool Sucesso { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }

}
