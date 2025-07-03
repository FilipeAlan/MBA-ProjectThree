namespace AlunoContext.Infrastructure.Context;

public class UnitOfWork : IAlunoUnitOfWork
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