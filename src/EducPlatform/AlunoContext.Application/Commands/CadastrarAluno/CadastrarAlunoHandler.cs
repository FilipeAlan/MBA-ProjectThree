using AlunoContext.Domain.Aggregates;
using AlunoContext.Domain.Common;
using AlunoContext.Domain.Repositories;
using BuildingBlocks.Results;

namespace AlunoContext.Application.Commands.CadastrarAluno;

public class CadastrarAlunoHandler
{
    private readonly IAlunoRepository _repositorio;
    private readonly IUsuarioContexto _usuarioContexto;

    public CadastrarAlunoHandler(IAlunoRepository repositorio, IUsuarioContexto usuarioContexto)
    {
        _repositorio = repositorio;
        _usuarioContexto = usuarioContexto;
    }

    public async Task<Result> Handle(CadastrarAlunoComando comando)
    {
        if (string.IsNullOrWhiteSpace(comando.Nome))
            return Result.Fail("O nome do aluno é obrigatório.");

        if (string.IsNullOrWhiteSpace(comando.Email) || !comando.Email.Contains('@'))
            return Result.Fail("O e-mail informado é inválido.");

        var aluno = new Aluno(comando.Nome, comando.Email, _usuarioContexto.ObterUsuario());
        
        await _repositorio.Adicionar(aluno);

        return Result.Ok("Aluno cadastrado com sucesso.");
    }

}
