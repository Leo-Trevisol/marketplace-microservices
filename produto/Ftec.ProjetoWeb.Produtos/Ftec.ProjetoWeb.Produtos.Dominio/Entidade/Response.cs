namespace Ftec.ProjetoWeb.Produtos.Dominio.Entidade
{
    public class Response<T>
    {
        public bool Sucesso { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public Response()
        {

        }
        public Response(bool status, T data, string mensagem)
        {

            this.Sucesso = status;
            this.Data = data;
            this.Message = mensagem;

        }
    }
}
