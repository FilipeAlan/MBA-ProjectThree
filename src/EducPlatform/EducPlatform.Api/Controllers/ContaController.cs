using BuildingBlocks.Security;
using EducPlatform.Api.Dtos;
using EducPlatform.Api.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EducPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IConfiguration _configuration;

        public ContaController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarUsuarioRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = new Usuario
            {
                Id = Guid.NewGuid(), // Cria o ID do usuário e também do aluno
                UserName = request.Email,
                Email = request.Email,
                NomeCompleto = request.Nome
            };

            var resultado = await _userManager.CreateAsync(usuario, request.Senha);

            if (!resultado.Succeeded)
                return BadRequest(resultado.Errors);

            // Sucesso ao registrar
            return Ok(new { message = "Usuário registrado com sucesso!" });
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginUsuarioRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _userManager.FindByEmailAsync(request.Email);

            if (usuario == null)
                return Unauthorized("Usuário ou senha inválidos.");

            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Senha, false);

            if (!resultado.Succeeded)
                return Unauthorized("Usuário ou senha inválidos.");

            var chaveJwt = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key não configurada.");
            var token = JwtTokenGenerator.GenerateToken(usuario.Id.ToString(), usuario.Email!, chaveJwt);

            return Ok(new LoginResponse
            {
                Token = token,
                ExpiraEm = DateTime.UtcNow.AddMinutes(60),
                Email = usuario.Email!,
                UsuarioId = usuario.Id
            });
        }
    }
}
