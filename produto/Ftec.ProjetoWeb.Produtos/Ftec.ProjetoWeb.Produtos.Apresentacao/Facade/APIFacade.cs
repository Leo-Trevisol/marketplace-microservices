using Ftec.ProjetoWeb.Produtos.Apresentacao.Enums;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API;
using Microsoft.AspNetCore.ResponseCompression;
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
                                var mediaResponse = Get<APIResponseModel<MediaModel>>($"api/Media/obterPorId/{item.IdImagemPrincipal}");
                                item.ImagemPrincipal = mediaResponse.Data;

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


        public List<ProdutoModel> BuscarProdutosPorTexto(string texto)
        {
            try
            {
                this.obtemBaseUrl(_config, TipoServico.Produto);
                var response = Get<APIResponseModel<List<ProdutoModel>>>($"api/produto/buscar/{texto}");

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
                                var mediaResponse = Get<APIResponseModel<MediaModel>>($"api/Media/obterPorId/{item.IdImagemPrincipal}");
                                item.ImagemPrincipal = mediaResponse.Data;

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
                Console.WriteLine(e);
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

                if (!string.IsNullOrEmpty(produto.IdImagemPrincipal.ToString()))
                {
                    this.obtemBaseUrl(_config, TipoServico.Produto);
                    var mediaResponse = Get<APIResponseModel<MediaModel>>($"api/Media/obterPorId/{produto.IdImagemPrincipal}");
                    produto.ImagemPrincipal = mediaResponse.Data;

                }
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
            Console.WriteLine(produto.Nome);
            this.obtemBaseUrl(_config, TipoServico.Produto); // garante que _baseUrl está setada
            var status = Post("api/Produto/cadastrarProduto", produto);

            Console.WriteLine(status);

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

        #region Media

        public async Task<MediaUploadModel?> UploadImagemAsync(IFormFile arquivo)
        {
            obtemBaseUrl(_config, TipoServico.Produto);

            using var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            using var form = new MultipartFormDataContent();

            using var stream = arquivo.OpenReadStream();

            form.Add(
                new StreamContent(stream),
                "arquivo",
                arquivo.FileName);

            var response = await client.PostAsync("api/media/upload", form);

            // 🔍 1. status code
            Console.WriteLine("===== UPLOAD RESPONSE STATUS =====");
            Console.WriteLine(response.StatusCode);

            // 🔍 2. JSON bruto (isso aqui é o mais importante)
            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine("===== UPLOAD RESPONSE JSON =====");
            Console.WriteLine(json);

            response.EnsureSuccessStatusCode();

            // 🔍 3. tentativa de desserializar
            var resultado = System.Text.Json.JsonSerializer.Deserialize<UploadMediaResponse>(
                json,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            Console.WriteLine("===== DATA OBJETO =====");
            Console.WriteLine($"Id: {resultado?.Data?.Id}");
            Console.WriteLine($"Caminho: {resultado?.Data?.CaminhoArquivo}");

            return resultado?.Data;
        }

        #endregion

        #region Categorias
        public List<CategoriaModel> ListarGeralCategorias(bool montaArvore = false)
        {
            try
            {
                this.obtemBaseUrl(_config, TipoServico.Categoria);
                var response = Get<APIResponseModel<List<CategoriaModel>>>("api/categoria/listar");
                return response.Data ?? new List<CategoriaModel>();
            }
            catch (Exception e)
            {
                return new List<CategoriaModel>();
            }
        }
        public List<CategoriaArvoreModel> ListarCategorias(bool montaArvore = false)
        {
            try
            {
                this.obtemBaseUrl(_config, TipoServico.Categoria);
                var response = Get<APIResponseModel<List<CategoriaArvoreModel>>>("api/categoria/arvore");
                return response.Data ?? new List<CategoriaArvoreModel>();

            }
            catch (Exception e)
            {
                return new List<CategoriaArvoreModel>();
            }
        }

        public APICategoriaModel ObterCategoriaGerenciar(int id)
        {
            this.obtemBaseUrl(_config, TipoServico.Categoria);
            var response = Get<APIResponseModel<APICategoriaModel>>($"api/Categoria/obterPorId/{id}");

            var categoria = response?.Data ?? new APICategoriaModel();

            return categoria;
        }

        public bool AdicionarCategoria(APICategoriaModel categoria)
        {
            this.obtemBaseUrl(_config, TipoServico.Categoria); // garante que _baseUrl está setada
            var status = Post("api/categoria/cadastrar", categoria);
            return status;
        }

        public bool AlterarCategoria(APICategoriaModel categoria)
        {
            this.obtemBaseUrl(_config, TipoServico.Categoria);
            var status = Put($"api/Produto/{categoria.Id}", categoria);
            return status;
        }

        public bool ExcluirCategoria(int id)
        {
            obtemBaseUrl(_config, TipoServico.Categoria);

            using (var client = new HttpClient())
            {
                var url = $"{_baseUrl.TrimEnd('/')}/api/categoria/excluir/{id}";

                var response = client.DeleteAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                {
                    var error = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(error);
                }

                return true;
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
        public bool AdicionarPedido(APIPedidoRegistrarModel modelo)
        {
            if (modelo != null)
            {
                this.obtemBaseUrl(_config, TipoServico.PedidosCarrinho);
                var status = Post($"api/Pedido", modelo);
                return status;
            }
            else
            {
                return false;
            }
        }
        public APIResponseModel<APIPedidoModel> ObterPedidoPorId(Guid Id)
        {
            this.obtemBaseUrl(_config, TipoServico.PedidosCarrinho);
            var pedido = Get<APIResponseModel<APIPedidoModel>>($"api/Pedido/{Id}");
            return pedido;
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

        #region Pagamento

        #endregion

        #region Estatísticas
        public List<APIEstatisticaDiariaModel> ObtemPainelDiario() {
            this.obtemBaseUrl(_config, TipoServico.Estatisticas);
            var painel = Get<List<APIEstatisticaDiariaModel>>($"api/Estatistica/painel-hoje");
            return painel;
        }
        public APIEstatisticaVendaGeral ObtemTotalVendas() {
            this.obtemBaseUrl(_config, TipoServico.Estatisticas);
            var totalVenda = Get<APIEstatisticaVendaGeral>($"api/Estatistica/total-vendas");
            return totalVenda;
        }
        public APIEstatisticaAvaliacao ObtemMediaAvaliacao() {
            this.obtemBaseUrl(_config, TipoServico.Estatisticas);
            var avaliacao = Get<APIEstatisticaAvaliacao>($"api/Estatistica/media-avaliacao-produto");
            return avaliacao;
        }
        public APIEstatisticaVendaProduto ObtemVendaProduto() {
            this.obtemBaseUrl(_config, TipoServico.Estatisticas);
            var vendaP = Get<APIEstatisticaVendaProduto>($"api/Estatistica/media-venda-produto");
            return vendaP;
        }
        public APIEstatisticaVendaCliente ObtemVendaCliente() {
            this.obtemBaseUrl(_config, TipoServico.Estatisticas);
            var vendaC = Get<APIEstatisticaVendaCliente>($"api/Estatistica/media-vendas-cliente");
            return vendaC;
        }
        #endregion

        public Guid? AdicionarPedidoRetornando(APIPedidoRegistrarModel modelo)
        {
            if (modelo != null)
            {
                this.obtemBaseUrl(_config, TipoServico.PedidosCarrinho);
                using (var client = new HttpClient())
                {
                    var url = $"{_baseUrl.TrimEnd('/')}/api/Pedido";
                    var jsonContent = JsonSerializer.Serialize(modelo);
                    var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, ContentType);

                    var response = client.PostAsync(url, content).Result;

                    if (response.IsSuccessStatusCode)
                        return modelo.id;
                    else
                    {
                        var erro = response.Content.ReadAsStringAsync().Result;
                        throw new Exception($"Erro ao adicionar pedido: {erro}");
                    }
                }
            }
            return null;
        }

        private TResponse PostComRetorno<TRequest, TResponse>(string endpoint, TRequest data)
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
                    throw new Exception($"Erro na requisição: {errorContent}");
                }

                var jsonResposta = response.Content.ReadAsStringAsync().Result;
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<TResponse>(jsonResposta, options);
            }
        }

        #region Pagamento
        public APIPagamentoModel RegistrarPagamento(APIPagamentoModel modelo)
        {
            if (modelo == null) return null;

            this.obtemBaseUrl(_config, TipoServico.Pagamento);
            try
            {
                return PostComRetorno<APIPagamentoModel, APIPagamentoModel>("api/pagamento", modelo);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public APIPagamentoTransacaoModel ProcessarTransacao(Guid pagamentoId, decimal valor)
        {
            this.obtemBaseUrl(_config, TipoServico.Pagamento);
            var request = new APIPagamentoTransacaoModel
            {
                pagamentoId = pagamentoId,
                valor = valor
            };

            try
            {
                return PostComRetorno<APIPagamentoTransacaoModel, APIPagamentoTransacaoModel>("api/pagamento/transacao", request);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Frete
        public APIFreteModel CalcularFrete(Guid pedidoId, string cepOrigem, string cepDestino)
        {
            this.obtemBaseUrl(_config, TipoServico.Pagamento);
            var request = new APIFreteCalcularModel
            {
                pedidoId = pedidoId,
                cepOrigem = cepOrigem,
                cepDestino = cepDestino
            };

            try
            {
                return PostComRetorno<APIFreteCalcularModel, APIFreteModel>("api/frete/calcular", request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FRETE] ERRO: {ex.Message}");
                return null;
            }
        }

        public APIFreteModel ObterFretePorPedido(Guid pedidoId)
        {
            this.obtemBaseUrl(_config, TipoServico.Pagamento);
            try
            {
                return Get<APIFreteModel>($"api/frete/pedido/{pedidoId}");
            }
            catch (Exception)
            {
                return null;
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
                case TipoServico.Pagamento:
                    this._baseUrl = config["PagamentoBaseUrl"];
                    break;
                case TipoServico.Estatisticas:
                    this._baseUrl = config["EstatisticaBaseUrl"];
                    break;
                default:
                    this._baseUrl = config["ProdutoBaseUrl"];
                    break;
            }
        }
        #endregion
    }
}