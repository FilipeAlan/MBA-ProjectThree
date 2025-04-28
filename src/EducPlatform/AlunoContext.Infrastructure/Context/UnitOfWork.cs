using BuildingBlocks.Common;

namespace AlunoContext.Infrastructure.Context;

public class UnitOfWork : IUnitOfWork
{
    private readonly AlunoDbContext _context;

    public UnitOfWork(AlunoDbContext context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }
}