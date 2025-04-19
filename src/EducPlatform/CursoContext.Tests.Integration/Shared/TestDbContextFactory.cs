using CursoContext.Infrastructure.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CursoContext.Tests.Integration.Shared;

public static class TestDbContextFactory
{
    private static SqliteConnection? _connection;

    public static CursoDbContext CriarContexto()
    {
        if (_connection == null)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
        }

        var options = new DbContextOptionsBuilder<CursoDbContext>()
            .UseSqlite(_connection)
            .Options;

        var contexto = new CursoDbContext(options);
        contexto.Database.EnsureCreated();

        return contexto;
    }
}
