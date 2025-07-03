using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Domain.Repositories;
using AlunoContext.Infrastructure.Context;
using AlunoContext.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using AlunoUnitOfWorkImpl = AlunoContext.Infrastructure.Context.UnitOfWork;
namespace EducPlatform.Api.Extensions
{
    public static class AlunoDependencyInjection
    {
        public static IServiceCollection AddAlunoContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AlunoDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAlunoRepository, AlunoRepository>();
            services.AddScoped<IAlunoUnitOfWork, AlunoUnitOfWorkImpl>();

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(CadastrarAlunoHandler).Assembly));

            return services;
        }
    }

}
