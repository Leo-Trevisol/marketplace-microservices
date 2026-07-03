using Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models {
    public class EstatisticaModel {
        public EstatisticaModel() {
            Estatisticas = new APIEstatisticaModel();
        }

        public APIEstatisticaModel Estatisticas { get; set; }
    }
}
