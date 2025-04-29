using AlunoContext.Infrastructure.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace PagamentoContext.Tests.Integration.Shared;

public static class AlunoTestDbContextFactory
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
