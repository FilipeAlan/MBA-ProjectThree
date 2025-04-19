using BuildingBlocks.Common;

namespace CursoContext.Tests.Shared.Fakes;

public class UnitOfWorkFake : IUnitOfWork
{
    public Task Commit() => Task.CompletedTask;
}
