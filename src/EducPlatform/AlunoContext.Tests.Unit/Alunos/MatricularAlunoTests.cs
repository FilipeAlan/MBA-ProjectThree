using AlunoContext.Application.Commands.MatricularAluno;
using AlunoContext.Domain.Entities;
using AlunoContext.Tests.Shared.Builders;
using AlunoContext.Tests.Shared.Fakes;
using AlunoContext.Tests.Unit.Fakes;

namespace AlunoContext.Tests.Unit.Alunos;

public class MatricularAlunoTests
{
    private readonly AlunoRepositorioFake _repositorioFake;
    private readonly UsuarioContextoFake _usuarioFake;
    private readonly UnitOfWorkFake _unitOfWorkFake;

    public MatricularAlunoTests()
    {
        _repositorioFake = new AlunoRepositorioFake();
        _usuarioFake = new UsuarioContextoFake();
        _unitOfWorkFake = new UnitOfWorkFake();
    }

    [Fact(DisplayName = "Deve matricular aluno quando dados forem válidos")]
    public async Task DeveMatricularAluno_ComDadosValidos()
    {
        // Arrange
        var aluno = AlunoBuilder.Novo().Construir();
        await _repositorioFake.Adicionar(aluno);
        var comando = new MatricularAlunoComando(aluno.Id, Guid.NewGuid());
        var handler = new MatricularAlunoHandler(_repositorioFake, _usuarioFake, new CursoConsultaFake(true), _unitOfWorkFake);

        // Act
        var resultado = await handler.Handle(comando);

        // Assert
        Assert.True(resultado.Sucesso);
        Assert.Single(aluno.Matriculas);
    }

    [Fact(DisplayName = "Não deve matricular aluno inexistente")]
    public async Task NaoDeveMatricular_AlunoInexistente()
    {
        // Arrange
        var comando = new MatricularAlunoComando(Guid.NewGuid(), Guid.NewGuid());
        var handler = new MatricularAlunoHandler(_repositorioFake, _usuarioFake, new CursoConsultaFake(true), _unitOfWorkFake);

        // Act
        var resultado = await handler.Handle(comando);

        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Contains("aluno não encontrado", resultado.Mensagem.ToLower());
    }

    [Fact(DisplayName = "Não deve matricular em curso inexistente")]
    public async Task NaoDeveMatricular_CursoInexistente()
    {
        // Arrange
        var aluno = AlunoBuilder.Novo().Construir();
        await _repositorioFake.Adicionar(aluno);
        var comando = new MatricularAlunoComando(aluno.Id, Guid.NewGuid());
        var handler = new MatricularAlunoHandler(_repositorioFake, _usuarioFake, new CursoConsultaFake(false), _unitOfWorkFake);

        // Act
        var resultado = await handler.Handle(comando);

        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Contains("curso não encontrado", resultado.Mensagem.ToLower());
    }

    [Fact(DisplayName = "Não deve permitir matrícula duplicada")]
    public async Task NaoDeveMatricular_SeJaMatriculado()
    {
        // Arrange
        var cursoId = Guid.NewGuid();
        var aluno = AlunoBuilder.Novo().Construir();
        aluno.AdicionarMatricula(new Matricula(cursoId, "TDD"));
        await _repositorioFake.Adicionar(aluno);
        var comando = new MatricularAlunoComando(aluno.Id, cursoId);
        var handler = new MatricularAlunoHandler(_repositorioFake, _usuarioFake, new CursoConsultaFake(true), _unitOfWorkFake);

        // Act
        var resultado = await handler.Handle(comando);

        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Contains("já está matriculado", resultado.Mensagem.ToLower());
        Assert.Single(aluno.Matriculas);
    }
}
