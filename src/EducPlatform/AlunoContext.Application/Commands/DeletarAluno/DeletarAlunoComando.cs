namespace AlunoContext.Application.Commands.DeletarAluno;

    public class DeletarAlunoComando
    {
        public Guid Id { get; }
        public DeletarAlunoComando(Guid id)
        {
            Id = id;
        }
    }

