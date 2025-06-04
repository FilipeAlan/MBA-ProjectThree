using System.ComponentModel.DataAnnotations;

namespace EducPlatform.Api.Dtos
{
    public class RegistrarUsuarioRequest
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "Senha deve ter pelo menos 6 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        public string Tipo { get; set; } = "admin"; // Padrão para admin, pode ser alterado conforme necessidade
    }
}
