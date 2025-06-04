using EducPlatform.Api.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EducPlatform.Api.Extensions
{
    public static class DatabaseInitializationExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            await context.Database.MigrateAsync();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            var email = "admin@admin.com";
            var user = await userManager.FindByEmailAsync(email);

            if (user != null && !await userManager.IsInRoleAsync(user, "admin"))
            {
                if (!await roleManager.RoleExistsAsync("admin"))
                    await roleManager.CreateAsync(new IdentityRole<Guid>("admin"));

                await userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}
