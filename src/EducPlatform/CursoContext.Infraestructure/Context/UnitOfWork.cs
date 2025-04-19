using BuildingBlocks.Common;

namespace CursoContext.Infrastructure.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CursoDbContext _context;

        public UnitOfWork(CursoDbContext context)
        {
            _context = context;
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
