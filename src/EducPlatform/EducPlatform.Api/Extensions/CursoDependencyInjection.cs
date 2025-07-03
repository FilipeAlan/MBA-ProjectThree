using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Application.Consumers;
using CursoContext.Domain.Repositories;
using CursoContext.Infrastructure.Context;
using CursoContext.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using CursoUnitOfWorkImpl = CursoContext.Infrastructure.Context.UnitOfWork;

namespace EducPlatform.Api.Extensions
{
    public static class CursoDependencyInjection
    {
        public static IServiceCollection AddCursoContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CursoDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICursoRepository, CursoRepository>();
            services.AddScoped<ICursoUnityOfWork, CursoUnitOfWorkImpl>();            

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(CadastrarCursoHandler).Assembly));

            return services;
        }
    }

}
