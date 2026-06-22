namespace Ftec.ProjetoWeb.Produtos.Apresentacao.Models.Auth
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = default!;
        public DateTime AccessTokenExpiresIn { get; set; }

        public string RefreshToken { get; set; } = default!;
        public DateTime RefreshTokenExpiresIn { get; set; }

        public Guid UsuarioId { get; set; }
        public string Nome { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
