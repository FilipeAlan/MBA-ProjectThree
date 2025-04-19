using AlunoContext.Domain.Entities;
using AlunoContext.Domain.Repositories;
using BuildingBlocks.Results;
using AlunoContext.Application.Common;
using BuildingBlocks.Common;

namespace AlunoContext.Application.Commands.MatricularAluno;

public class MatricularAlunoHandler
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly IUsuarioContexto _usuarioContexto;
    private readonly ICursoConsulta _cursoConsulta;
    private readonly IUnitOfWork _unitOfWork;

    public MatricularAlunoHandler(
        IAlunoRepository alunoRepository,
        IUsuarioContexto usuarioContexto,
        ICursoConsulta cursoConsulta,
        IUnitOfWork unitOfWork)
    {
        _alunoRepository = alunoRepository;
        _usuarioContexto = usuarioContexto;
        _cursoConsulta = cursoConsulta;
        _unitOfWork = unitOfWork;
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
        await _unitOfWork.Commit();
        return Result.Ok("Aluno matriculado com sucesso.");
    }
}
