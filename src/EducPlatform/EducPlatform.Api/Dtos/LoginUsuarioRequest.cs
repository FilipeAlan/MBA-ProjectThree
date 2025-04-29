using System.ComponentModel.DataAnnotations;

namespace EducPlatform.Api.Dtos
{
    public class LoginUsuarioRequest
    {
        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória.")]
        public string Senha { get; set; } = string.Empty;
    }
}
