namespace CursoContext.Infrastructure.Context
{
    public class UnitOfWork : ICursoUnityOfWork
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
