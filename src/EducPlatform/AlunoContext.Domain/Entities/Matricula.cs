using AlunoContext.Domain.Enums;
using BuildingBlocks.Common;
using CursoContext.Domain.Entities;

namespace AlunoContext.Domain.Entities;

public class Matricula : EntityBase
{
    public Guid CursoId { get; private set; }
    public DateTime DataMatricula { get; private set; }
    public StatusMatricula Status { get; private set; }

    protected Matricula() : base("SYSTEM") { }

    public Matricula(Guid cursoId, string usuarioCriacao)
        : base(usuarioCriacao)
    {
        CursoId = cursoId;
        DataMatricula = DateTime.UtcNow;
        Status = StatusMatricula.Pendente;
    }

    public void ConfirmarPagamento(string usuario)
    {
        if (Status != StatusMatricula.Pendente)
            throw new InvalidOperationException("Somente matrículas pendentes podem ser ativadas.");

        Status = StatusMatricula.Ativa;
        Atualizar(usuario);
    }

    public void Finalizar(string usuario)
    {
        if (Status != StatusMatricula.Ativa)
            throw new InvalidOperationException("Somente matrículas ativas podem ser concluídas.");

        Status = StatusMatricula.Concluida;
        Atualizar(usuario);
    }
}
