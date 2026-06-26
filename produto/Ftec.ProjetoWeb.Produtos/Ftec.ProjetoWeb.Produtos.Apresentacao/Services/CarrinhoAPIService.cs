using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using System.Text.Json;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Services {
    public class CarrinhoAPIService {
        private const string CarrinhoSessionKey = "Carrinho";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CarrinhoAPIService(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public List<CarrinhoModel> ObterCarrinho() {
            var json = Session.GetString(CarrinhoSessionKey);
            return json == null
                ? new List<CarrinhoModel>()
                : JsonSerializer.Deserialize<List<CarrinhoModel>>(json);
        }

        public void AdicionarItem(CarrinhoModel item) {
            var carrinho = ObterCarrinho();
            var existente = carrinho.FirstOrDefault(i => i.IdProduto == item.IdProduto);

            if (existente != null)
                existente.Quantidade += item.Quantidade;
            else
                carrinho.Add(item);

            SalvarCarrinho(carrinho);
        }

        public void RemoverItem(Guid produtoId) {
            var carrinho = ObterCarrinho();
            carrinho.RemoveAll(i => i.IdProduto == produtoId);
            SalvarCarrinho(carrinho);
        }

        private void SalvarCarrinho(List<CarrinhoModel> carrinho) {
            Session.SetString(CarrinhoSessionKey, JsonSerializer.Serialize(carrinho));
        }
        public void LimparCarrinho() {
            Session.Remove(CarrinhoSessionKey);
        }
    }
}
