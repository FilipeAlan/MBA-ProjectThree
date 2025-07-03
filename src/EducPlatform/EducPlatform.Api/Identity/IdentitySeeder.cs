using Microsoft.AspNetCore.Identity;

namespace EducPlatform.Api.Identity
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            const string adminEmail = "admin@admin.com";
            const string adminPassword = "Admin@123";
            const string adminRole = "admin";
            const string alunoRole = "aluno"; 

            // Cria a role ADMIN se não existir
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(adminRole));
            }

            // Cria a role ALUNO se não existir
            if (!await roleManager.RoleExistsAsync(alunoRole))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(alunoRole));
            }

            // Cria o usuário admin se não existir
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new Usuario
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
                else
                {
                    throw new InvalidOperationException("Erro ao criar usuário admin: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                // Garante que esteja na role admin
                if (!await userManager.IsInRoleAsync(adminUser, adminRole))
                    await userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }
    }
}
