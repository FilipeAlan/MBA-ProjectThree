using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PagamentoContext.Infrastructure.Context;

namespace PagamentoContext.Tests.Integration.Shared;

public static class PagamentoTestDbContextFactory
{
    public static PagamentoDbContext CriarContexto()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<PagamentoDbContext>()
            .UseSqlite(connection)
            .Options;

        var contexto = new PagamentoDbContext(options);
        contexto.Database.EnsureCreated();

        return contexto;
    }
}
