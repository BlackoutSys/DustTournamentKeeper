using DustTournamentKeeper.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustTournamentKeeper.Infrastructure
{
    internal static class TestDataGenerator
    {
        public static void GenerateClubs(int number, ITournamentRepository repository)
        {
            for (int i = 0; i < number; i++)
            {
                var club = new Club()
                {
                    Address = $"Address{i}",
                    City = $"City{i}",
                    Name = $"Club{i}",
                    Country = $"Country{i}"
                };
                repository.Add(club);
            }
        }

        public static async Task GenerateAdmin(ITournamentRepository repository, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            var admin = repository.Users.FirstOrDefault(u => u.UserName == "Admin");
            if (admin != null) return;

            var roleId = repository.Roles.Where(r => r.Name == nameof(Roles.Administrator)).Select(r => r.Id).FirstOrDefault().ToString();
            var applicationRole = await roleManager.FindByIdAsync(roleId).ConfigureAwait(false);

            if (applicationRole == null)
            {
                return;
            }

            var user = new User()
            {
                Name = $"Admin",
                UserName = $"Admin",
                Surname = $"Admin",
                Email = $"admin@email.com",
                Country = $"Country1",
                City = "City2",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            IdentityResult result = await userManager.CreateAsync(user, "K4k@o123456").ConfigureAwait(false);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, applicationRole.NormalizedName).ConfigureAwait(false);
            }
        }

        public static async Task GenerateUsers(int number, ITournamentRepository repository, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            Random rand = new Random();

            var roleId = repository.Roles.Where(r => r.Name == nameof(Roles.User)).Select(r => r.Id).FirstOrDefault().ToString();
            var applicationRole = await roleManager.FindByIdAsync(roleId).ConfigureAwait(false);

            if (applicationRole == null)
            {
                return;
            }

            for (int i = 0; i < number; i++)
            {
                var user = new User()
                {
                    Name = $"Name{i}",
                    UserName = $"UserName{i}",
                    Surname = $"Surname{i}",
                    Email = $"Email{i}@email.com",
                    Country = $"Country1",
                    City = i%2 > 0 ? $"City{i}" : "City2",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                if (i % 2 > 0)
                {
                    var availableClubs = repository.Clubs.ToList();
                    user.ClubId = availableClubs[rand.Next(0, availableClubs.Count)].Id;
                }

                IdentityResult result = await userManager.CreateAsync(user, "K4k@o123456").ConfigureAwait(false);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, applicationRole.Name).ConfigureAwait(false);
                }
            }
        }

        public static void GenerateBoards(int number, ITournamentRepository repository)
        {
            for (int i = 0; i < number; i++)
            {
                var board = new BoardType()
                {
                    Name = $"BoardType{i}"
                };

                repository.Add(board);
            }
        }

        public async static Task GenerateTournaments(int number, ITournamentRepository repository, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            Random rand = new Random();

            var organizer = repository.Users.FirstOrDefault(u => u.UserName == "Admin");
            if (organizer == null)
            {
                await GenerateAdmin(repository, userManager, roleManager);
            }

            var organizerId = repository.Users.FirstOrDefault(u => u.UserName == "Admin").Id;
            var factionsAvailable = repository.Factions.ToList();
            var blocksAvailable = repository.Blocks.ToList();
            var clubsAvailable = repository.Clubs.ToList();
            var boardsAvailable = repository.BoardTypes.ToList();
            var playersAvailable = repository.Users.ToList();

            for (int i = 0; i < number; i++)
            {
                var tournament = new Tournament()
                {
                    DateStart = DateTime.Today.AddDays(1).AddHours(9),
                    DateEnd = DateTime.Today.AddDays(1).AddHours(17),
                    Country = $"Country1",
                    City = i % 2 > 0 ? $"City{i}" : "City2",
                    Address = i % 2 > 0 ? $"Address{i}" : "Address2",
                    OrganizerId = organizerId,
                    Title = $"Title{i}",
                    PlayerLimit = rand.Next(6, 24),
                    Status = nameof(TournamentStatus.Pending),
                    Rounds = rand.Next(3, 5),
                    ArmyPoints = rand.Next(80, 150),
                    Fee = rand.Next(5, 30),
                    FeeCurrency = "$",
                    Bpwin = 3,
                    Bptie = 2,
                    Bploss = 1,
                    GameId = repository.Games.FirstOrDefault().Id
                };

                if (i % 2 > 0)
                {
                    tournament.ClubId = clubsAvailable[rand.Next(0, clubsAvailable.Count)].Id;
                }

                repository.Add(tournament);

                var playersNumberAllowed = rand.Next(5, tournament.PlayerLimit);
                var playerIds = new List<int>();

                for (int j = 0; j < playersNumberAllowed; j++)
                {
                    var blockId = blocksAvailable[rand.Next(0, blocksAvailable.Count)].Id;
                    int? factionId = factionsAvailable.FirstOrDefault(f => f.BlockId == blockId)?.Id;
                    var tournamentUser = new TournamentUser()
                    {
                        BlockId = blockId,
                        FactionId = i % 2 > 0 ? factionId : null,
                        TournamentId = tournament.Id,
                        UserId = playersAvailable.Find(p => !playerIds.Contains(p.Id)).Id
                    };
                    playerIds.Add(tournamentUser.UserId);

                    repository.Add(tournamentUser);
                }

                var count = Math.Ceiling((double)playersNumberAllowed / 2);
                for (int k = 0; k < count; k++)
                {
                    var boardTypeId = boardsAvailable[rand.Next(0, boardsAvailable.Count)].Id;
                    var tournamentBoard = new TournamentBoardType()
                    {
                        Number = k + 1,
                        TournamentId = tournament.Id,
                        BoardTypeId = boardTypeId
                    };

                    repository.Add(tournamentBoard);
                }
            }
        }

        public async static Task NukeDb(ITournamentRepository repository, UserManager<User> userManager)
        {
            foreach (var match in repository.Matches.ToList())
            {
                repository.Delete(match);
            }

            foreach (var round in repository.Rounds.ToList())
            {
                repository.Delete(round);
            }

            foreach (var tournamentUser in repository.TournamentUsers.ToList())
            {
                repository.Delete(tournamentUser);
            }

            foreach (var tournamentBoardType in repository.TournamentBoardTypes.ToList())
            {
                repository.Delete(tournamentBoardType);
            }

            foreach (var tournament in repository.Tournaments.ToList())
            {
                repository.Delete(tournament);
            }

            foreach (var user in repository.Users.ToList())
            {
                if (!await userManager.IsInRoleAsync(user, "Administrator").ConfigureAwait(false))
                {
                    await userManager.DeleteAsync(user).ConfigureAwait(false);
                }
            }

            foreach (var club in repository.Clubs.ToList())
            {
                repository.Delete(club);
            }

            foreach (var board in repository.BoardTypes.ToList())
            {
                repository.Delete(board);
            }
        }
    }
}
