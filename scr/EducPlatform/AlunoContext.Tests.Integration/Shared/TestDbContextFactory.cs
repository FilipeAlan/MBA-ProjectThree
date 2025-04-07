using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using AlunoContext.Infrastructure.Context;

namespace AlunoContext.Tests.Integration.Shared;

public static class TestDbContextFactory
{
    public static AlunoDbContext CriarContexto()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<AlunoDbContext>()
            .UseSqlite(connection)
            .Options;

        var contexto = new AlunoDbContext(options);
        contexto.Database.EnsureCreated();

        return contexto;
    }
}
