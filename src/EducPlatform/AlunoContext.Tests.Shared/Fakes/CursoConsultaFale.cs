using AlunoContext.Application.Common;

namespace AlunoContext.Tests.Shared.Fakes;

public class CursoConsultaFake : ICursoConsulta
{
    private readonly bool _existe;

    public CursoConsultaFake(bool existe = true)
    {
        _existe = existe;
    }

    public bool Existe(Guid cursoId)
    {
        return _existe;
    }
}
