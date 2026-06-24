using Ftec.ProjetoWeb.Produtos.Apresentacao.Enums;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Facade
{
    public class APIFacade
    {

        private string _baseUrl = string.Empty;
        private readonly HttpClient _httpClient;
        private const string ContentType = "application/json";
        private IConfiguration _config;
        public APIFacade(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _config = config;
        }

        #region Produto
        public List<ProdutoModel> ListarProdutos()
        {
            try
            {
                this.obtemBaseUrl(_config, TipoServico.Produto);
                var response = Get<APIResponseModel<List<ProdutoModel>>>("api/produto/listar");

                var produtos = response != null && response.Sucesso && response?.Data != null ? response?.Data : new List<ProdutoModel>();

                if (produtos != null && produtos.Count() > 0)
                {
                    foreach (var item in produtos)
                    {
                        if (!string.IsNullOrEmpty(item.IdImagemPrincipal.ToString()))
                        {
                            this.obtemBaseUrl(_config, TipoServico.Produto);
                            item.ImagemPrincipal = Get<MediaModel>($"api/Media/obterPorId/{item.IdImagemPrincipal}");
                        }
                        //if (item.IdCategoria.HasValue) {
                        //    this.obtemBaseUrl(_config, TipoServico.Categoria);
                        //    item.Categoria = Get<CategoriaModel>($"api/Categoria/{item.IdCategoria.Value}");
                        //}

                    }
                    return produtos;
                }
                else
                {
                    return new List<ProdutoModel>();
                }
            }
            catch (Exception e)
            {
                return new List<ProdutoModel>();
            }
        }
        public ProdutoModel ObterProduto(string id)
        {
            this.obtemBaseUrl(_config, TipoServico.Produto);
            var response = Get<APIResponseModel<ProdutoModel>>($"api/Produto/obtemPorId/{id}");

            var produto = response != null && response.Sucesso && response?.Data != null ? response.Data : new ProdutoModel();
            if (produto != null)
            {
                //if (produto.IdCategoria.HasValue) {
                //    this.obtemBaseUrl(_config, TipoServico.Categoria);
                //    produto.Categoria = Get<CategoriaModel>($"api/Categoria/{produto.IdCategoria.Value}");
                //}
                //if (!string.IsNullOrEmpty(produto.Id.ToString())) {
                //    this.obtemBaseUrl(_config, TipoServico.Avaliacao);
                //    produto.Avaliacoes = Get<List<ProdutoAvaliacaoModel>>($"api/ProdutoAvaliacao/GetAvaliacoesProduto/{produto.Id}");
                //}
            }

            return produto;
        }
        public void AdicionarProduto(ProdutoModel produto)
        {
            Post("api/Produto/cadastrarProduto", produto);
        }
        public void AlterarProduto(ProdutoModel produto)
        {
            Put("api/Produto/atualizarProduto", produto);
        }
        public void ExcluirProduto(string id)
        {
            Delete($"api/Produto/excluirProduto{id}");
        }
        #endregion

        #region Categorias
        public List<CategoriaModel> ListarCategorias() {
            try {
                this.obtemBaseUrl(_config, TipoServico.Categoria);
                var response = Get<APIResponseModel<List<CategoriaModel>>>("api/categoria");

                var categorias = response != null && response.Sucesso && response?.Data != null ? response?.Data : new List<CategoriaModel>();

                if (categorias != null && categorias.Count() > 0) {
                    return categorias;
                } else {
                    return new List<CategoriaModel>();
                }
            } catch (Exception e) {
                return new List<CategoriaModel>();
            }
        }
        #endregion

        #region Avaliacoes
        public void AdicionarAvaliacao(ProdutoAvaliacaoModel modelo) {
            if(modelo != null) {
                this.obtemBaseUrl(_config, TipoServico.Avaliacao);
                Post($"api/ProdutoAvaliacao/post/", modelo);
            }
        }
        #endregion

        #region Métodos privados de comunicação HTTP
        private T Get<T>(string endpoint)
        {
            using (var client = new HttpClient())
            {
                var url = string.Empty;
                url = $"{_baseUrl}{endpoint}";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType));

                var response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = response.Content.ReadAsStringAsync().Result;
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<T>(jsonContent, options);
                }
                else
                {
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new Exception($"Erro ao buscar dados: {errorContent}");
                }
            }
        }
        private void Post<T>(string endpoint, T data)
        {
            using (var client = new HttpClient())
            {
                var url = $"{_baseUrl}/{endpoint}";
                var jsonContent = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, ContentType);

                var response = client.PostAsync(url, content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new Exception($"Erro ao adicionar dados: {errorContent}");
                }
            }
        }
        private void Put<T>(string endpoint, T data)
        {
            using (var client = new HttpClient())
            {
                var url = $"{_baseUrl}/{endpoint}";
                var jsonContent = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, ContentType);

                var response = client.PutAsync(url, content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new Exception($"Erro ao alterar dados: {errorContent}");
                }
            }
        }
        private void Delete(string endpoint)
        {
            using (var client = new HttpClient())
            {
                var url = $"{_baseUrl}/{endpoint}";
                var response = client.DeleteAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new Exception($"Erro ao excluir dados: {errorContent}");
                }
            }
        }
        #endregion

        #region Helpers
        public void obtemBaseUrl(IConfiguration config, TipoServico tipo)
        {
            var baseUrl = string.Empty;
            switch (tipo)
            {
                case TipoServico.Produto:
                    this._baseUrl = config["ProdutoBaseUrl"];
                    break;
                case TipoServico.Categoria:
                    this._baseUrl = config["CategoriaBaseUrl"];
                    break;
                case TipoServico.Avaliacao:
                    this._baseUrl = config["AvaliacaoBaseUrl"];
                    break;
                case TipoServico.Usuarios:
                    this._baseUrl = config["UsuarioBaseUrl"];
                    break;
                case TipoServico.PedidosCarrinho:
                    this._baseUrl = config["PedidoCarrinhoBaseUrl"];
                    break;
                default:
                    this._baseUrl = config["ProdutoBaseUrl"];
                    break;
            }
        }
        #endregion
    }
}
