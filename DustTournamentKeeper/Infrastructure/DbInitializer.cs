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

                // Dust specific settings
                var repository = serviceScope.ServiceProvider.GetRequiredService<ITournamentRepository>();

                // Game
                var games = repository.Games.ToList();
                if (games.FirstOrDefault(g => g.Name == "Dust1947") == null)
                {
                    var game = new Game
                    {
                        Name = "Dust1947"
                    };
                    repository.Add(game);
                }

                // Blocks
                var dustGame = repository.Games.FirstOrDefault(g => g.Name == "Dust1947");
                var blocks = repository.Blocks.ToList();
                if (blocks.FirstOrDefault(b => b.Name == "Axis") == null)
                {
                    var block = new Block
                    {
                        Name = "Axis",
                        GameId = dustGame.Id
                    };
                    repository.Add(block);
                }

                if (blocks.FirstOrDefault(b => b.Name == "Allies") == null)
                {
                    var block = new Block
                    {
                        Name = "Allies",
                        GameId = dustGame.Id
                    };
                    repository.Add(block);
                }

                if (blocks.FirstOrDefault(b => b.Name == "SSU") == null)
                {
                    var block = new Block
                    {
                        Name = "SSU",
                        GameId = dustGame.Id
                    };
                    repository.Add(block);
                }

                if (blocks.FirstOrDefault(b => b.Name == "Mercenaries") == null)
                {
                    var block = new Block
                    {
                        Name = "Mercenaries",
                        GameId = dustGame.Id
                    };
                    repository.Add(block);
                }

                if (blocks.FirstOrDefault(b => b.Name == "IJN") == null)
                {
                    var block = new Block
                    {
                        Name = "IJN",
                        GameId = dustGame.Id
                    };
                    repository.Add(block);
                }

                // Factions
                var factions = repository.Factions.ToList();

                var axisBlock = repository.Blocks.FirstOrDefault(b => b.Name == "Axis");
                if (factions.FirstOrDefault(f => f.Name == "Luftwaffe") == null)
                {
                    var faction = new Faction
                    {
                        Name = "Luftwaffe",
                        GameId = dustGame.Id,
                        BlockId = axisBlock.Id
                    };
                    repository.Add(faction);
                }

                if (factions.FirstOrDefault(f => f.Name == "NDAK") == null)
                {
                    var faction = new Faction
                    {
                        Name = "NDAK",
                        GameId = dustGame.Id,
                        BlockId = axisBlock.Id
                    };
                    repository.Add(faction);
                }

                if (factions.FirstOrDefault(f => f.Name == "Blutkreuz") == null)
                {
                    var faction = new Faction
                    {
                        Name = "Blutkreuz",
                        GameId = dustGame.Id,
                        BlockId = axisBlock.Id
                    };
                    repository.Add(faction);
                }

                var alliesBlock = repository.Blocks.FirstOrDefault(b => b.Name == "Allies");
                if (factions.FirstOrDefault(f => f.Name == "Marines") == null)
                {
                    var faction = new Faction
                    {
                        Name = "Marines",
                        GameId = dustGame.Id,
                        BlockId = alliesBlock.Id
                    };
                    repository.Add(faction);
                }

                if (factions.FirstOrDefault(f => f.Name == "Desert Scorpions") == null)
                {
                    var faction = new Faction
                    {
                        Name = "Desert Scorpions",
                        GameId = dustGame.Id,
                        BlockId = alliesBlock.Id
                    };
                    repository.Add(faction);
                }

                var ssuBlock = repository.Blocks.FirstOrDefault(b => b.Name == "SSU");
                if (factions.FirstOrDefault(f => f.Name == "Spetsnaz") == null)
                {
                    var faction = new Faction
                    {
                        Name = "Spetsnaz",
                        GameId = dustGame.Id,
                        BlockId = ssuBlock.Id
                    };
                    repository.Add(faction);
                }

                if (factions.FirstOrDefault(f => f.Name == "Red Guard") == null)
                {
                    var faction = new Faction
                    {
                        Name = "Red Guard",
                        GameId = dustGame.Id,
                        BlockId = ssuBlock.Id
                    };
                    repository.Add(faction);
                }

                if (factions.FirstOrDefault(f => f.Name == "PLA") == null)
                {
                    var faction = new Faction
                    {
                        Name = "PLA",
                        GameId = dustGame.Id,
                        BlockId = ssuBlock.Id
                    };
                    repository.Add(faction);
                }
            }
        }
    }
}
