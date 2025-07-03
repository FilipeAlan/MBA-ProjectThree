using AlunoContext.Infrastructure.Context;
using CursoContext.Infrastructure.Context;
using EducPlatform.Api.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PagamentoContext.Infrastructure.Context;

namespace EducPlatform.Api.Extensions
{
    public static class DatabaseInitializationExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            // Aplica migrations do Identity
            var identityContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            await identityContext.Database.MigrateAsync();

            // Aplica migrations do contexto do Aluno
            var alunoContext = scope.ServiceProvider.GetRequiredService<AlunoDbContext>();
            await alunoContext.Database.MigrateAsync();

            // Aplica migrations do contexto do Aluno
            var cursoContext = scope.ServiceProvider.GetRequiredService<CursoDbContext>();
            await cursoContext.Database.MigrateAsync();

            // Aplica migrations do contexto do Aluno
            var pagamentoContext = scope.ServiceProvider.GetRequiredService<PagamentoDbContext>();
            await pagamentoContext.Database.MigrateAsync();

            // Cria usuário admin e associa à role ADMIN se necessário
            await IdentitySeeder.SeedAsync(scope.ServiceProvider);
        }
    }

}
