using AlunoContext.Infrastructure.Context;

namespace AlunoContext.Tests.Shared.Fakes;

public class UnitOfWorkFake : IAlunoUnitOfWork
{
    public Task Commit() => Task.CompletedTask;
}
