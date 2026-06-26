using System.Text.Json;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Services {
    public class CarrinhoItem {
        public Guid IdProduto { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
    }
    public class CarrinhoAPIService {
        private const string CarrinhoSessionKey = "Carrinho";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CarrinhoAPIService(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public List<CarrinhoItem> ObterCarrinho() {
            var json = Session.GetString(CarrinhoSessionKey);
            return json == null
                ? new List<CarrinhoItem>()
                : JsonSerializer.Deserialize<List<CarrinhoItem>>(json);
        }

        public void AdicionarItem(CarrinhoItem item) {
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

        private void SalvarCarrinho(List<CarrinhoItem> carrinho) {
            Session.SetString(CarrinhoSessionKey, JsonSerializer.Serialize(carrinho));
        }
    }
}
