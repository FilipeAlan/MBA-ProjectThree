using Microsoft.AspNetCore.Identity;

namespace EducPlatform.Api.Identity
{
    public class Usuario : IdentityUser<Guid>
    {
        public string NomeCompleto { get; set; } = string.Empty;
    }
}
