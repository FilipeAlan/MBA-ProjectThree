using AlunoContext.Domain.Entities;

namespace PagamentoContext.Tests.Shared.Builders;

public class MatriculaBuilder
{
    private Guid _cursoId = Guid.NewGuid();
    private string _usuarioCriacao = "system";

    public MatriculaBuilder ComCurso(Guid cursoId)
    {
        _cursoId = cursoId;
        return this;
    }

    public MatriculaBuilder ComUsuario(string usuario)
    {
        _usuarioCriacao = usuario;
        return this;
    }

    public Matricula Construir()
    {
        return new Matricula(_cursoId, _usuarioCriacao);
    }
}
