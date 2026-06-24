using Ftec.ProjetoWeb.Produtos.Apresentacao.Models.API;
using Ftec.ProjetoWeb.Produtos.Apresentacao.Models.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuthApiService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            _httpClient.BaseAddress = new Uri(
                _configuration["AuthApi:BaseUrl"]!);
        }

        public async Task<APIResponseModel<string>> CadastrarUsuarioAsync(UsuarioModel usuario)
        {
            bool success = false;
            var response = await _httpClient.PostAsJsonAsync(
                "/api/usuario",
                new
                {
                    nome = usuario.Nome,
                    email = usuario.Email,
                    senha = usuario.Senha,
                    documento = usuario.Documento,
                    tipoPessoa = usuario.TipoPessoa,
                    funcao = usuario.Funcao,
                    dataNascimento = usuario.DataNascimento.ToString("yyyy-MM-dd"),
                    telefone = usuario.Telefone
                });

            if (response.StatusCode == HttpStatusCode.Created) {
                success = true;
            }

            var conteudo = await response.Content.ReadAsStringAsync();

            return new APIResponseModel<string>() {
                Sucesso = success,
                Data = $"Status: {(int)response.StatusCode}\n{conteudo}",
                Message = ""
            };
        }

        public async Task<LoginResponseDto?> LoginAsync(string email, string senha)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/api/autenticacao/login",
                new
                {
                    email,
                    senha
                });

            var conteudo = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // aqui ainda vamos melhorar depois, por enquanto só retorna null
                return null;
            }

            var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(
                conteudo,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return loginResponse;
        }

        public async Task LogoutAsync(string refreshToken)
        {
            await _httpClient.PostAsJsonAsync(
                "/api/autenticacao/logout",
                new
                {
                    token = refreshToken
                });
        }
    }
}
