using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PagamentoContext.Domain.Aggregates;
using PagamentoContext.Infrastructure.Context;

namespace PagamentoContext.Tests.Performance;

public class RealizarPagamentoPerformanceTests
{
    private readonly PagamentoDbContext _context;
    private readonly UnitOfWork _unitOfWork;

    public RealizarPagamentoPerformanceTests()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<PagamentoDbContext>()
            .UseSqlite(connection)
            .Options;

        _context = new PagamentoDbContext(options);
        _context.Database.EnsureCreated();

        _unitOfWork = new UnitOfWork(_context);
    }

    [Fact(DisplayName = "Deve cadastrar 1000 pagamentos em menos de 20 segundos")]
    public async Task DeveCadastrarPagamentosRapidamente()
    {
        // Arrange
        var pagamentos = new List<Pagamento>();

        for (int i = 0; i < 1000; i++)
        {
            var dadosCartao = new Domain.ValueObjects.DadosCartao(
                "4111111111111111",
                "Aluno Performance",
                "12/29",
                "123"
            );

            var pagamento = new Pagamento(Guid.NewGuid(), 2000m, dadosCartao);
            pagamento.ConfirmarPagamento();
            pagamentos.Add(pagamento);
        }

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        await _context.Pagamentos.AddRangeAsync(pagamentos);
        await _unitOfWork.Commit(); // <-- Agora usando UnitOfWork!

        stopwatch.Stop();

        // Assert
        Assert.True(stopwatch.Elapsed.TotalSeconds <= 5, $"Cadastro demorou {stopwatch.Elapsed.TotalSeconds} segundos");
    }
    [Fact(DisplayName = "Deve listar 1000 pagamentos em menos de 2000ms")]
    public async Task DeveListarPagamentosRapidamente()
    {
        // Arrange: garantir que já existam 1000 pagamentos
        if (!await _context.Pagamentos.AnyAsync())
        {
            var pagamentos = new List<Pagamento>();

            for (int i = 0; i < 1000; i++)
            {
                var dadosCartao = new Domain.ValueObjects.DadosCartao(
                    "4111111111111111",
                    "Aluno Performance",
                    "12/29",
                    "123"
                );

                var pagamento = new Pagamento(Guid.NewGuid(), 2000m, dadosCartao);
                pagamento.ConfirmarPagamento();
                pagamentos.Add(pagamento);
            }

            await _context.Pagamentos.AddRangeAsync(pagamentos);
            await _unitOfWork.Commit();
        }

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var pagamentosListados = await _context.Pagamentos.ToListAsync();

        stopwatch.Stop();

        // Assert
        Assert.Equal(1000, pagamentosListados.Count);
        Assert.True(stopwatch.Elapsed.TotalMilliseconds <= 500, $"Listagem demorou {stopwatch.Elapsed.TotalMilliseconds} ms");
    }
}
