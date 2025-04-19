using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using CursoContext.Infrastructure.Context;

namespace CursoContext.Tests.Integration.Shared;

public static class TestDbContextFactory
{
    public static CursoDbContext CriarContexto()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<CursoDbContext>()
            .UseSqlite(connection)
            .Options;

        var contexto = new CursoDbContext(options);
        contexto.Database.EnsureCreated();

        return contexto;
    }
}
