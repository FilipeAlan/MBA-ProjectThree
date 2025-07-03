using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Common
{
    public class UsuarioContexto : IUsuarioContexto
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioContexto(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string ObterUsuario()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";
        }
    }
}
