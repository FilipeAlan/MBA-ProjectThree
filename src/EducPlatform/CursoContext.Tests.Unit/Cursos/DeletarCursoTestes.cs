using CursoContext.Application.Commands.DeletarCurso;
using CursoContext.Tests.Shared.Builders;
using CursoContext.Tests.Shared.Fakes;
using CursoContext.Tests.Unit.Fakes;

namespace CursoContext.Tests.Unit.Cursos;

public class DeletarCursoTests
{
    [Fact(DisplayName = "Deve remover curso quando o ID for válido")]
    public async Task aaa_DeveRemoverCurso_Existente()
    {
        var curso = CursoBuilder.Novo().Construir();
        var repositorio = new CursoRepositorioFake();
        var unitOfWork = new UnitOfWorkFake();
        await repositorio.Adicionar(curso);

        var handler = new DeletarCursoHandler(repositorio, unitOfWork);
        var comando = new DeletarCursoComando(curso.Id);

        var resultado = await handler.Handle(comando, CancellationToken.None);

        Assert.True(resultado.Sucesso);
        Assert.Empty(repositorio.Cursos);
    }

    [Fact(DisplayName = "Não deve remover curso quando o ID não existir")]
    public async Task aaa_NaoDeveRemoverCurso_Inexistente()
    {
        var repositorio = new CursoRepositorioFake();
        var unitOfWork = new UnitOfWorkFake();
        var handler = new DeletarCursoHandler(repositorio, unitOfWork);
        var comando = new DeletarCursoComando(Guid.NewGuid());

        var resultado = await handler.Handle(comando, CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.Contains("curso não encontrado", resultado.Mensagem.ToLower());
    }
}
