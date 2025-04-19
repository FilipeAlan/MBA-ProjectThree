namespace AlunoContext.Application.Commands.MatricularAluno;
public class MatricularAlunoComando
{
    public Guid AlunoId { get; }
    public Guid CursoId { get; }
    public MatricularAlunoComando(Guid alunoId, Guid cursoId)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
    }
}

