using DustTournamentKeeper.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DustTournamentKeeper.Infrastructure
{
    public static class DbInitializer
    {
        public static async Task Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

                // Roles preparation
                var roles = roleManager.Roles.ToList();
                if (roles.FirstOrDefault(r => r.Name == nameof(Roles.Administrator)) == null)
                {
                    await roleManager.CreateAsync(new Role(nameof(Roles.Administrator)));
                }
                if (roles.FirstOrDefault(r => r.Name == nameof(Roles.Organizer)) == null)
                {
                    await roleManager.CreateAsync(new Role(nameof(Roles.Organizer)));
                }
                if (roles.FirstOrDefault(r => r.Name == nameof(Roles.User)) == null)
                {
                    await roleManager.CreateAsync(new Role(nameof(Roles.User)));
                }

                // Admin user preparation
                var admin = await userManager.FindByNameAsync("Admin");
                if (admin == null)
                {
                    admin = new User()
                    {
                        Name = $"Admin",
                        UserName = $"Admin",
                        Surname = $"Admin",
                        Email = $"admin@email.com",
                        Country = $"Poland",
                        City = "Warsaw",
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    IdentityResult result = await userManager.CreateAsync(admin, "K4k@o123456").ConfigureAwait(false);
                    if (result.Succeeded)
                    {
                        var applicationRole = await roleManager.FindByNameAsync(nameof(Roles.Administrator));
                        if (applicationRole != null)
                        {
                            IdentityResult roleResult = await userManager.AddToRoleAsync(admin, applicationRole.NormalizedName);
                        }
                    }
                }
                else
                {
                    var isAdmin = await userManager.IsInRoleAsync(admin, nameof(Roles.Administrator));
                    if (!isAdmin)
                    {
                        var applicationRole = await roleManager.FindByNameAsync(nameof(Roles.Administrator));
                        if (applicationRole != null)
                        {
                            IdentityResult roleResult = await userManager.AddToRoleAsync(admin, applicationRole.NormalizedName);
                        }
                    }
                }
            }
        }
    }
}
