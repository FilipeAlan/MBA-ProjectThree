namespace EducPlatform.Api.Dtos
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiraEm { get; set; }
        public string Email { get; set; } = string.Empty;
        public Guid UsuarioId { get; set; }
    }
}
