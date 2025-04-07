namespace AlunoContext.Application.Queries.ObterAluno;

public class ObterAlunoQuery
{
    public Guid Id { get; }

    public ObterAlunoQuery(Guid id)
    {
        Id = id;
    }
}
