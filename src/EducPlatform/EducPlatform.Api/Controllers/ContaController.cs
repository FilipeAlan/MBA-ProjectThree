namespace EducPlatform.Api.Controllers;

using AlunoContext.Application.Commands.CadastrarAluno;
using BuildingBlocks.Security;
using EducPlatform.Api.Dtos;
using EducPlatform.Api.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MediatR;


[ApiController]
[Route("api/[controller]")]
public class ContaController : ControllerBase
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;

    public ContaController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IConfiguration configuration, IMediator mediator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _mediator = mediator;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegistrarUsuarioRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            UserName = request.Email,
            Email = request.Email,
            NomeCompleto = request.Nome
        };

        var resultado = await _userManager.CreateAsync(usuario, request.Senha);
        if (!resultado.Succeeded)
            return BadRequest(resultado.Errors);

        var role = request.Tipo?.ToLower() == "admin" ? "admin" : "aluno";

        if (!await _userManager.IsInRoleAsync(usuario, role))
            await _userManager.AddToRoleAsync(usuario, role);

        // Se for aluno, criar também a entidade Aluno
        if (role == "aluno")
        {
            var comando = new CadastrarAlunoComando(
                usuario.Id,
                usuario.NomeCompleto,
                usuario.Email!
            );

            var resultadoAluno = await _mediator.Send(comando);
            if (!resultadoAluno.Sucesso)
                return BadRequest(resultadoAluno.Mensagem);
        }

        return Ok($"Usuário ({role}) criado com sucesso.");
    }


    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginUsuarioRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var usuario = await _userManager.FindByEmailAsync(request.Email);
        if (usuario == null) return Unauthorized("Usuário ou senha inválidos.");

        var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Senha, false);
        if (!resultado.Succeeded) return Unauthorized("Usuário ou senha inválidos.");

        var chaveJwt = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key não configurada.");

        var roles = (await _userManager.GetRolesAsync(usuario)).ToList();

        var token = JwtTokenGenerator.GenerateToken(usuario.Id.ToString(), usuario.Email!, roles, chaveJwt);

        return Ok(new LoginResponse
        {
            Token = token,
            ExpiraEm = DateTime.UtcNow.AddMinutes(60),
            Email = usuario.Email!,
            UsuarioId = usuario.Id
        });
    }
}
