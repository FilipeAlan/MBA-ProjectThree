namespace CursoContext.Application.Commands.DeletarCurso;

public class DeletarCursoComando
{
    public Guid Id { get; }

    public DeletarCursoComando(Guid id)
    {
        Id = id;
    }
}
