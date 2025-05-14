using CursoContext.Application.Commands.CadastrarAula;
using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Infrastructure.Context;
using CursoContext.Infrastructure.Repositories;
using CursoContext.Tests.Integration.Shared;
using CursoContext.Tests.Shared.Fakes;

namespace CursoContext.Tests.Integration.Curso;

public class CadastrarAulaIntegrationTests
{
    [Fact(DisplayName = "Deve cadastrar aula e associar ao curso no banco de dados")]
    public async Task DeveCadastrarAula_ComSucesso()
    {
        using (var contexto = TestDbContextFactory.CriarContexto())
        {
            var repositorio = new CursoRepository(contexto);
            var usuario = new UsuarioContextoFake();
            var unitOfWork = new UnitOfWork(contexto);

            // 1. Criar o curso
            var cursoHandler = new CadastrarCursoHandler(repositorio, usuario, unitOfWork);
            var resultadoCurso = await cursoHandler.Handle(new CadastrarCursoComando("Curso de TDD", "Aprenda TDD"),CancellationToken.None);
            Assert.True(resultadoCurso.Sucesso);


            var cursoId = resultadoCurso.Dados;

            // 2. Cadastrar a aula
            var aulaHandler = new CadastrarAulaHandler(repositorio, usuario, unitOfWork);
            var resultadoAula = await aulaHandler.Handle(new CadastrarAulaComando(cursoId, "Aula 01", "Conteúdo de TDD"),CancellationToken.None);
            Assert.True(resultadoAula.Sucesso);

          
            var cursoAtualizado = await repositorio.ObterPorId(cursoId);
         
            Assert.NotNull(cursoAtualizado);
          
            Assert.NotEmpty(cursoAtualizado!.Aulas);
           
            Assert.Single(cursoAtualizado.Aulas);
            Assert.Equal("Aula 01", cursoAtualizado.Aulas.FirstOrDefault()?.Titulo);
        }
    }
}
