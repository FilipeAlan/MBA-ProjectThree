namespace CursoContext.Application.Queries.ObterCurso;

public class ObterCursoPorIdQuery
{
    public Guid Id { get; }

    public ObterCursoPorIdQuery(Guid id)
    {
        Id = id;
    }
}
