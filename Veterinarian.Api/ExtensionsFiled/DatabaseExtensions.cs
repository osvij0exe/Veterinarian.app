using Microsoft.AspNetCore.Identity;
using Veterinaria.Domain.Entities.Users;

namespace Veterinarian.Api.ExtensionsFiled
{
    public static class DatabaseExtensions
    {
        public static async Task SeedInitialDataAsync(this WebApplication app)
        {
            using IServiceScope scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new List<string>
            {
                Role.Admin,
                Role.AuxiliaryMember,
                Role.VetMember, // Corregido typo
                Role.Owner
            };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            app.Logger.LogError("Error creating role {Role}: {Error}", roleName, error.Description);
                        }
                    }
                    else
                    {
                        app.Logger.LogInformation("Role {Role} created successfully.", roleName);
                    }
                }
                else
                {
                    app.Logger.LogInformation("Role {Role} already exists.", roleName);
                }
            }
        }
    }
}
