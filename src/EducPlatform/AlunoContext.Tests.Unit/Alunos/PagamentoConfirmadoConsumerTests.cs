using AlunoContext.Application.Consumers;
using AlunoContext.Domain.Aggregates;
using AlunoContext.Domain.Entities;
using AlunoContext.Domain.Enums;
using AlunoContext.Domain.Repositories;
using BuildingBlocks.Common;
using BuildingBlocks.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AlunoContext.Tests.Unit.Alunos;

public class PagamentoConfirmadoConsumerTests
{
    [Fact(DisplayName = "Deve processar mensagem de pagamento confirmado e ativar matrícula")]
    public async Task DeveProcessarMensagemPagamentoConfirmado()
    {
        // Arrange
        var alunoId = Guid.NewGuid();
        var matriculaId = Guid.NewGuid();
        var aluno = new Aluno(alunoId, "Teste", "teste@email.com", "system");
        var matricula = new Matricula(Guid.NewGuid(), "system");
        typeof(Matricula).GetProperty(nameof(Matricula.Id))!.SetValue(matricula, matriculaId);
        aluno.AdicionarMatricula(matricula);

        var alunoRepoMock = new Mock<IAlunoRepository>();
        alunoRepoMock.Setup(x => x.ObterAlunoPorMatriculaId(matriculaId))
                     .ReturnsAsync(aluno);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var scopeMock = new Mock<IServiceScope>();
        var providerMock = new Mock<IServiceProvider>();

        providerMock.Setup(x => x.GetService(typeof(IAlunoRepository))).Returns(alunoRepoMock.Object);
        providerMock.Setup(x => x.GetService(typeof(IUnitOfWork))).Returns(unitOfWorkMock.Object);
        scopeMock.Setup(x => x.ServiceProvider).Returns(providerMock.Object);

        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);

        var loggerMock = new Mock<ILogger<PagamentoConfirmadoConsumer>>();

        var connectionFactory = new ConnectionFactory(); // Dummy para o teste
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c.GetSection("RabbitMQ")["Queue"]).Returns("fila-teste");

        var consumer = new PagamentoConfirmadoConsumer(
            scopeFactoryMock.Object,
            loggerMock.Object,
            connectionFactory,
            configMock.Object);

        var eventPayload = new PagamentoConfirmadoEvent(matriculaId);
        var json = JsonSerializer.Serialize(eventPayload);
        var body = Encoding.UTF8.GetBytes(json);

        // Emular o processamento direto (via método exposto no teste)
        await consumer.TestarProcessamentoDireto(body);

        // Assert
        alunoRepoMock.Verify(x => x.Atualizar(It.Is<Aluno>(a => a.Id == aluno.Id)), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
        Assert.Equal(StatusMatricula.Ativa, matricula.Status);
    }
}
