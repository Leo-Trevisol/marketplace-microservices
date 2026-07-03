namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API
{
    public class APIFreteModel
    {
        public Guid idFrete { get; set; }
        public Guid pedidoId { get; set; }
        public decimal valorFrete { get; set; }
        public string statusEntrega { get; set; }
        public string cepDestino { get; set; }
    }

    public class APIFreteCalcularModel
    {
        public Guid pedidoId { get; set; }
        public string cepOrigem { get; set; }
        public string cepDestino { get; set; }
    }
}