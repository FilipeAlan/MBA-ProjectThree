using CursoContext.Infrastructure.Context;

namespace CursoContext.Tests.Shared.Fakes;

public class UnitOfWorkFake : ICursoUnityOfWork
{
    public Task Commit() => Task.CompletedTask;
}
