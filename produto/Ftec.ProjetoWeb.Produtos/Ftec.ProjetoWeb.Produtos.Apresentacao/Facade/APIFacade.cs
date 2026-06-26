using Ftec.ProjetoWeb.Produtos.Apresentacao.Enums;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API;
using System.Net.Http.Headers;
using System.Reflection;
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
                        try
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
                        catch (Exception ex)
                        {
                            continue;
                        }
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
                if (!string.IsNullOrEmpty(produto.Id.ToString()))
                {
                    this.obtemBaseUrl(_config, TipoServico.Avaliacao);
                    produto.Avaliacoes = Get<List<ProdutoAvaliacaoModel>>($"api/avaliacao/produto/{produto.Id}");
                }
            }

            return produto;
        }
        public APIProdutoModel ObterProdutoGerenciar(string id)
        {
            this.obtemBaseUrl(_config, TipoServico.Produto);
            var response = Get<APIResponseModel<APIProdutoModel>>($"api/Produto/obtemPorId/{id}");

            var produto = response != null && response.Sucesso && response?.Data != null ? response.Data : new APIProdutoModel();
            if (produto != null)
            {
                //if (produto.IdCategoria.HasValue) {
                //    this.obtemBaseUrl(_config, TipoServico.Categoria);
                //    produto.Categoria = Get<CategoriaModel>($"api/Categoria/{produto.IdCategoria.Value}");
                //}
            }

            return produto;
        }

        public bool AdicionarProduto(APIProdutoModel produto)
        {
            this.obtemBaseUrl(_config, TipoServico.Produto); // garante que _baseUrl está setada
            var status = Post("api/Produto/cadastrarProduto", produto);
            return status;
        }

        public bool AlterarProduto(APIProdutoModel produto)
        {
            this.obtemBaseUrl(_config, TipoServico.Produto);
            var status = Put("api/Produto/atualizarProduto", produto);
            return status;
        }

        public bool ExcluirProduto(Guid id)
        {
            this.obtemBaseUrl(_config, TipoServico.Produto);
            var status = Delete($"api/Produto/excluirProduto/{id}");
            return status;
        }
        #endregion

        #region Categorias

        public List<CategoriaModel> ListarCategorias()
        {
            try
            {
                this.obtemBaseUrl(_config, TipoServico.Categoria);
                var response = Get<List<CategoriaModel>>("api/categoria");
                return response ?? new List<CategoriaModel>();
            }
            catch (Exception e)
            {
                return new List<CategoriaModel>();
            }
        }

        public APICategoriaModel ObterCategoriaGerenciar(string id)
        {
            //this.obtemBaseUrl(_config, TipoServico.Categoria);
            //var response = Get<APICategoriaModel>($"api/Categoria/{id}");

            int.TryParse(id, out int idPretendido);
            if (idPretendido == 0) idPretendido = 2; // Fallback caso venha nulo/vazio

            // Simula o retorno com base no ID solicitado
            var categoria = new APICategoriaModel
            {
                Id = idPretendido,
                Nome = idPretendido == 1 ? "Eletrônicos" : idPretendido == 2 ? "Computadores" : "Smartphones",
                Descricao = idPretendido == 1
                    ? "Aparelhos e dispositivos eletrônicos em geral"
                    : "Notebooks, desktops e servidores de alta performance",
                ParentId = idPretendido == 1 ? null : 1 // Se for 1 é raiz, senão o pai é o id 1
            };

            return categoria;
        }

        public bool AdicionarCategoria(APICategoriaModel categoria)
        {
            this.obtemBaseUrl(_config, TipoServico.Categoria); // garante que _baseUrl está setada
            var status = Post("api/Categoria", categoria);
            return status;
        }

        public bool AlterarCategoria(APICategoriaModel categoria)
        {
            this.obtemBaseUrl(_config, TipoServico.Categoria);
            var status = Put($"api/Produto/{categoria.Id}", categoria);
            return status;
        }

        public bool ExcluirCategoria(Guid id)
        {
            this.obtemBaseUrl(_config, TipoServico.Categoria);
            var status = Delete($"api/Produto/{id}");
            return status;
        }

        public List<CategoriaListaViewModel> ListarCategoriasView()
        {
            try
            {
                this.obtemBaseUrl(_config, TipoServico.Categoria);

                // 1. Busca a lista plana vinda da API
                var response = Get<List<CategoriaModel>>("api/categoria");
                var listaPlana = response ?? new List<CategoriaModel>();

                var listaOrganizada = new List<CategoriaListaViewModel>();

                // 2. Filtra as categorias "Raiz" (que não têm pai)
                var raizes = listaPlana
                    .Where(c => c.ParentId == null || c.ParentId == 0)
                    .OrderBy(c => c.Nome)
                    .ToList();

                // 3. Dispara a organização recursiva para cada raiz encontrada
                foreach (var raiz in raizes)
                {
                    MapearFilhosRecursivo(raiz, listaPlana, listaOrganizada, nivel: 0);
                }

                return listaOrganizada;
            }
            catch (Exception e)
            {
                // Em caso de erro, retorna a lista vazia no formato correto da View
                return new List<CategoriaListaViewModel>();
            }
        }

        private void MapearFilhosRecursivo(CategoriaModel atual, List<CategoriaModel> todas, List<CategoriaListaViewModel> resultado, int nivel)
        {
            // Busca o nome do pai direto na lista original
            var nomePai = todas.FirstOrDefault(p => p.Id == atual.ParentId)?.Nome ?? "Nenhum";

            // Transforma no seu ViewModel populando o nível atual de indentação
            resultado.Add(new CategoriaListaViewModel
            {
                Id = atual.Id,
                Nome = atual.Nome,
                Descricao = atual.Descricao,
                ParentId = atual.ParentId,
                Nivel = nivel,
                CategoriaPaiNome = nomePai
            });

            // Busca os filhos que apontam para o Id da categoria atual
            var filhos = todas
                .Where(c => c.ParentId == atual.Id)
                .OrderBy(c => c.Nome)
                .ToList();

            // Entra no próximo nível (aumentando o nível do recuo)
            foreach (var filho in filhos)
            {
                MapearFilhosRecursivo(filho, todas, resultado, nivel + 1);
            }
        }

        #endregion

        #region Avaliacoes
        public bool AdicionarAvaliacao(APIProdutoAvaliacaoModel modelo)
        {
            if (modelo != null)
            {
                this.obtemBaseUrl(_config, TipoServico.Avaliacao);
                var status = Post($"api/avaliacao", modelo);
                return status;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Pedidos
        public bool AdicionarPedido(APIPedidoModel modelo) {
            if (modelo != null) {
                this.obtemBaseUrl(_config, TipoServico.PedidosCarrinho);
                var status = Post($"api/Pedido", modelo);
                return status;
            } else {
                return false;
            }
        }
        #endregion

        #region Métodos privados de comunicação HTTP
        private T Get<T>(string endpoint)
        {
            using (var client = new HttpClient())
            {
                var url = $"{_baseUrl.TrimEnd('/')}/{endpoint}";

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

        private bool Post<T>(string endpoint, T data)
        {
            using (var client = new HttpClient())
            {
                var url = $"{_baseUrl.TrimEnd('/')}/{endpoint}";
                var jsonContent = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, ContentType);

                var response = client.PostAsync(url, content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new Exception($"Erro ao adicionar dados: {errorContent}");
                    return false;
                }
                return true;
            }
        }

        private bool Put<T>(string endpoint, T data)
        {
            using (var client = new HttpClient())
            {
                var url = $"{_baseUrl.TrimEnd('/')}/{endpoint}";
                var jsonContent = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, ContentType);

                var response = client.PutAsync(url, content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new Exception($"Erro ao alterar dados: {errorContent}");
                }
                return true;
            }
        }

        private bool Delete(string endpoint)
        {
            using (var client = new HttpClient())
            {
                var url = $"{_baseUrl.TrimEnd('/')}/{endpoint}";
                var response = client.DeleteAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new Exception($"Erro ao excluir dados: {errorContent}");
                }
                return true;
            }
        }
        #endregion

        #region Helpers
        public void obtemBaseUrl(IConfiguration config, TipoServico tipo)
        {
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