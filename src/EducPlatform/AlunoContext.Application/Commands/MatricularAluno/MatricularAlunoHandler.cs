using AlunoContext.Application.Commands.MatricularAluno;
using AlunoContext.Domain.Entities;
using AlunoContext.Domain.Repositories;
using AlunoContext.Domain.Common;
using BuildingBlocks.Results;
using AlunoContext.Application.Common;

namespace AlunoContext.Application.Handlers;

public class MatricularAlunoHandler
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly IUsuarioContexto _usuarioContexto;
    private readonly ICursoConsulta _cursoConsulta;

    public MatricularAlunoHandler(
        IAlunoRepository alunoRepository,
        IUsuarioContexto usuarioContexto,
        ICursoConsulta cursoConsulta)
    {
        _alunoRepository = alunoRepository;
        _usuarioContexto = usuarioContexto;
        _cursoConsulta = cursoConsulta;
    }


    public async Task<Result> Handle(MatricularAlunoComando comando)
    {
        var aluno = await _alunoRepository.ObterPorId(comando.AlunoId);
        if (aluno is null)
            return Result.Fail("Aluno não encontrado.");

        if (!_cursoConsulta.Existe(comando.CursoId))
            return Result.Fail("Curso não encontrado.");

        var jaMatriculado = aluno.Matriculas.Any(m => m.CursoId == comando.CursoId);
        if (jaMatriculado)
            return Result.Fail("Aluno já está matriculado nesse curso.");

        var matricula = new Matricula(comando.CursoId, _usuarioContexto.ObterUsuario());
        aluno.AdicionarMatricula(matricula);

        await _alunoRepository.Atualizar(aluno);

        return Result.Ok("Aluno matriculado com sucesso.");
    }
}
