using AlunoContext.Domain.Aggregates;
using AlunoContext.Domain.Entities;
using AlunoContext.Domain.Repositories;
using AlunoContext.Infrastructure.Context;
using AlunoContext.Infrastructure.Repositories;
using BuildingBlocks.Common;
using Microsoft.EntityFrameworkCore;
using PagamentoContext.Applicarion.Commands;
using PagamentoContext.Domain.Enums;
using PagamentoContext.Domain.Repositories;
using PagamentoContext.Infrastructure.Context;
using PagamentoContext.Infrastructure.Repositories;
using PagamentoContext.Tests.Integration.Shared;

namespace PagamentoContext.Tests.Integration.Pagamentos;

public class RealizarPagamentoIntegrationTests : IAsyncLifetime
{
    private PagamentoDbContext _pagamentoContext = null!;
    private AlunoDbContext _alunoContext = null!;
    private RealizarPagamentoHandler _handler = null!;
    private IPagamentoRepository _pagamentoRepository = null!;
    private IAlunoRepository _alunoRepository = null!;
    private IUnitOfWork _unitOfWork = null!;

    public Task InitializeAsync()
    {
        _pagamentoContext = PagamentoTestDbContextFactory.CriarContexto();
        _alunoContext = AlunoTestDbContextFactory.CriarContexto();

        _pagamentoRepository = new PagamentoRepository(_pagamentoContext);
        _alunoRepository = new AlunoRepository(_alunoContext);
        _unitOfWork = new PagamentoContext.Infrastructure.Context.UnitOfWork(_pagamentoContext);

        _handler = new RealizarPagamentoHandler(_pagamentoRepository, _alunoRepository, _unitOfWork);

        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _pagamentoContext.Database.EnsureDeletedAsync();
        await _alunoContext.Database.EnsureDeletedAsync();
        await _pagamentoContext.DisposeAsync();
        await _alunoContext.DisposeAsync();
    }

    [Fact(DisplayName = "Deve confirmar pagamento e ativar matrícula no banco")]
    public async Task DeveConfirmarPagamentoEAtivarMatricula_ComSucesso()
    {
        // Arrange
        var cursoId = Guid.NewGuid();
        var aluno = new Aluno("Aluno Integração", "teste@integracao.com", "system");

        var matricula = new Matricula(cursoId, "system");
        aluno.AdicionarMatricula(matricula);

        _alunoContext.Alunos.Add(aluno);
        await _alunoContext.SaveChangesAsync();

        var comando = new RealizarPagamentoComando
        {
            MatriculaId = matricula.Id,
            Valor = 500m,
            NumeroCartao = "1234567890", // termina com 0 = pagamento aprovado
            NomeTitular = "Aluno Integração",
            Validade = "12/29",
            CVV = "123"
        };

        // Act
        var resultado = await _handler.Handle(comando);

        // Assert
        Assert.True(resultado.Sucesso);

        var pagamento = await _pagamentoContext.Pagamentos.FirstOrDefaultAsync(p => p.MatriculaId == matricula.Id);
        Assert.NotNull(pagamento);
        Assert.Equal(StatusPagamento.Pago, pagamento!.Status);

        var alunoDb = await _alunoContext.Alunos
            .Include(a => a.Matriculas)
            .FirstOrDefaultAsync(a => a.Id == aluno.Id);

        var matriculaDb = alunoDb?.Matriculas.FirstOrDefault(m => m.Id == matricula.Id);
        Assert.NotNull(matriculaDb);
        Assert.Equal(AlunoContext.Domain.Enums.StatusMatricula.Ativa, matriculaDb!.Status);
    }
    [Fact(DisplayName = "Deve rejeitar pagamento e manter matrícula pendente")]
    public async Task DeveRejeitarPagamentoEManterMatriculaPendente()
    {
        // Arrange
        var cursoId = Guid.NewGuid();
        var aluno = new Aluno("Aluno Integração 2", "teste2@integracao.com", "system");

        var matricula = new Matricula(cursoId, "system");
        aluno.AdicionarMatricula(matricula);

        _alunoContext.Alunos.Add(aluno);
        await _alunoContext.SaveChangesAsync();

        var comando = new RealizarPagamentoComando
        {
            MatriculaId = matricula.Id,
            Valor = 500m,
            NumeroCartao = "9876543215", // termina com 5 = pagamento rejeitado
            NomeTitular = "Aluno Integração 2",
            Validade = "11/30",
            CVV = "321"
        };

        // Act
        var resultado = await _handler.Handle(comando);

        // Assert
        Assert.True(resultado.Sucesso);

        var pagamento = await _pagamentoContext.Pagamentos.FirstOrDefaultAsync(p => p.MatriculaId == matricula.Id);
        Assert.NotNull(pagamento);
        Assert.Equal(StatusPagamento.Rejeitado, pagamento!.Status);

        var alunoDb = await _alunoContext.Alunos
            .Include(a => a.Matriculas)
            .FirstOrDefaultAsync(a => a.Id == aluno.Id);

        var matriculaDb = alunoDb?.Matriculas.FirstOrDefault(m => m.Id == matricula.Id);
        Assert.NotNull(matriculaDb);
        Assert.Equal(AlunoContext.Domain.Enums.StatusMatricula.Pendente, matriculaDb!.Status);
    }

}
