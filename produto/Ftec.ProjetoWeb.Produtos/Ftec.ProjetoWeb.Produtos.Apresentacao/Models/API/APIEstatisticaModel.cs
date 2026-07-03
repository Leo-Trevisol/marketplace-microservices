namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API {
    public class APIEstatisticaModel {
        public APIEstatisticaModel() {
            PainelDiario = new List<APIEstatisticaDiariaModel>();
        }
        public List<APIEstatisticaDiariaModel> PainelDiario { get; set; }
        public APIEstatisticaAvaliacao Avaliacao { get; set; }
        public APIEstatisticaVendaProduto VendaProduto { get; set; }
        public APIEstatisticaVendaCliente VendaCliente { get; set; }
        public APIEstatisticaVendaGeral VendaGeral { get; set; }
    }
    public class APIEstatisticaDiariaModel {
        public APIEstatisticaDiariaModel() {

        }
        public string titulo { get; set; }
        public string valor { get; set; }
        public string detalhe { get; set; }
    }
    public class APIEstatisticaAvaliacao {

    }
    public class APIEstatisticaVendaProduto {

    }
    public class APIEstatisticaVendaCliente {

    }
    public class APIEstatisticaVendaGeral {
        public APIEstatisticaVendaGeral() {

        }
        public int totalPedidos { get; set; }
        public decimal totalVendas { get; set; }
    }
}
