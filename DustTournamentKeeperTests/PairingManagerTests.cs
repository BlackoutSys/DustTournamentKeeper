using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DustTournamentKeeper.Tests
{
    public class PairingManagerTests: IDisposable
    {
        private ITournamentRepository _dataRepository;
        private DustTournamentKeeperContext _fakeContext;

        public PairingManagerTests()
        {
            PrepareContextData();
        }

        /// <summary>
        /// Prepares fresh repository
        /// </summary>
        private void PrepareContextData(bool makePlayersNumberEven = false)
        {
            _fakeContext = new DustTournamentKeeperContext(CreateNewContextOptions());
            _dataRepository = new SqlTournamentRepository(_fakeContext);

            var club1 = new Club()
            {
                City = "City1",
                Name = "Club1"
            };

            var club2 = new Club()
            {
                City = "City2",
                Name = "Club2"
            };

            _dataRepository.Add(club1);
            _dataRepository.Add(club2);

            var user1 = new User()
            {
                City = "City1",
                ClubId = club1.Id,
                Email = "email1",
                Name = "Name1",
                UserName = "Nickname1",
                Country = "Country1",
                Surname = "Surname1"
            };

            var user2 = new User()
            {
                City = "City1",
                Email = "email2",
                Name = "Name2",
                UserName = "Nickname2",
                Country = "Country1",
                Surname = "Surname2"
            };

            var user3 = new User()
            {
                City = "City2",
                Email = "email3",
                Name = "Name3",
                UserName = "Nickname3",
                Country = "Country1",
                Surname = "Surname3"
            };

            var user4 = new User()
            {
                City = "City2",
                ClubId = club2.Id,
                Email = "email4",
                Name = "Name4",
                UserName = "Nickname4",
                Country = "Country1",
                Surname = "Surname4"
            };

            var user5 = new User()
            {
                City = "City2",
                ClubId = club2.Id,
                Email = "email5",
                Name = "Name5",
                UserName = "Nickname5",
                Country = "Country1",
                Surname = "Surname5"
            };

            var user6 = new User()
            {
                City = "City3",
                Email = "email6",
                Name = "Name6",
                UserName = "Nickname6",
                Country = "Country1",
                Surname = "Surname6"
            };

            _dataRepository.Add(user1);
            _dataRepository.Add(user2);
            _dataRepository.Add(user3);
            _dataRepository.Add(user4);
            _dataRepository.Add(user5);
            _dataRepository.Add(user6);

            var game = new Game()
            {
                Description = "Desc1",
                Name = "Game1"
            };

            _dataRepository.Add(game);

            var block1 = new Block()
            {
                GameId = game.Id,
                Name = "Block1"
            };

            var block2 = new Block()
            {
                GameId = game.Id,
                Name = "Block2"
            };

            var block3 = new Block()
            {
                GameId = game.Id,
                Name = "Block3"
            };

            _dataRepository.Add(block1);
            _dataRepository.Add(block2);
            _dataRepository.Add(block3);

            var faction1 = new Faction()
            {
                BlockId = block1.Id,
                GameId = game.Id,
                Name = "Faction1"
            };

            var faction2 = new Faction()
            {
                BlockId = block1.Id,
                GameId = game.Id,
                Name = "Faction2"
            };

            var faction3 = new Faction()
            {
                BlockId = block1.Id,
                GameId = game.Id,
                Name = "Faction3"
            };

            _dataRepository.Add(faction1);
            _dataRepository.Add(faction2);
            _dataRepository.Add(faction3);

            var board1 = new BoardType()
            {
                Name = "Board1"
            };

            var board2 = new BoardType()
            {
                Name = "Board2"
            };

            var board3 = new BoardType()
            {
                Name = "Board3"
            };

            _dataRepository.Add(board1);
            _dataRepository.Add(board2);
            _dataRepository.Add(board3);

            var tournament = new Tournament()
            {
                Address = "Address1",
                ArmyPoints = 100,
                Bploss = 1,
                Bptie = 3,
                Bpwin = 5,
                City = "City1",
                Country = "Country1",
                DateStart = DateTime.UtcNow.AddDays(1),
                DateEnd = DateTime.UtcNow.AddDays(1).AddHours(4),
                Fee = 10,
                FeeCurrency = "Euro",
                GameId = game.Id,
                OrganizerId = user1.Id,
                PlayerLimit = 6,
                Rounds = 3,
                Status = "Pending",
                Title = "Title1"
            };

            _dataRepository.Add(tournament);

            var userToTournament1 = new TournamentUser()
            {
                UserId = user1.Id,
                TournamentId = tournament.Id,
                BlockId = block1.Id,
                FactionId = faction1.Id
            };

            var userToTournament2 = new TournamentUser()
            {
                UserId = user2.Id,
                TournamentId = tournament.Id,
                BlockId = block1.Id,
                FactionId = faction1.Id
            };

            var userToTournament3 = new TournamentUser()
            {
                UserId = user3.Id,
                TournamentId = tournament.Id,
                BlockId = block1.Id,
                FactionId = faction2.Id
            };

            var userToTournament4 = new TournamentUser()
            {
                UserId = user4.Id,
                TournamentId = tournament.Id,
                BlockId = block1.Id,
                FactionId = faction2.Id
            };

            var userToTournament5 = new TournamentUser()
            {
                UserId = user5.Id,
                TournamentId = tournament.Id,
                BlockId = block2.Id
            };

            _dataRepository.Add(userToTournament1);
            _dataRepository.Add(userToTournament2);
            _dataRepository.Add(userToTournament3);
            _dataRepository.Add(userToTournament4);
            _dataRepository.Add(userToTournament5);

            if (makePlayersNumberEven)
            {
                var userToTournament6 = new TournamentUser()
                {
                    UserId = user6.Id,
                    TournamentId = tournament.Id,
                    BlockId = block2.Id
                };
                _dataRepository.Add(userToTournament6);
            }

            var boardToTournament1 = new TournamentBoardType()
            {
                BoardTypeId = board1.Id,
                TournamentId = tournament.Id,
                Number = 1
            };

            var boardToTournament2 = new TournamentBoardType()
            {
                BoardTypeId = board1.Id,
                TournamentId = tournament.Id,
                Number = 2
            };

            var boardToTournament3 = new TournamentBoardType()
            {
                BoardTypeId = board2.Id,
                TournamentId = tournament.Id,
                Number = 3
            };

            _dataRepository.Add(boardToTournament1);
            _dataRepository.Add(boardToTournament2);
            _dataRepository.Add(boardToTournament3);
        }

        /// <summary>
        /// Creates fresh InMemory database instance
        /// </summary>
        /// <returns>Context options</returns>
        private DbContextOptions<DustTournamentKeeperContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<DustTournamentKeeperContext>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        [Theory]
        [InlineData(1, 6, 2, 5, 3, 4)]
        [InlineData(1, 2, 3, 4, 5, 6)]
        [InlineData(1, 3, 2, 4, 5, 6)]
        [InlineData(1, 4, 2, 3, 5, 6)]
        [InlineData(1, 5, 2, 3, 4, 6)]
        [InlineData(1, 6, 2, 3, 4, 5)]
        [InlineData(1, 5, 3, 4, 2, null)]
        [InlineData(1, 2, 3, 4, 5, null)]
        [InlineData(1, 3, 4, 5, 2, null)]
        [InlineData(1, 4, 5, 6, 3, null)]
        [InlineData(1, 6, 2, 5, 4, null)]
        public void ResolveHorrorMatchesTest(int player1, int? player2, int player3, int? player4, int player5, int? player6)
        {
            PrepareContextData(true);

            var match1 = new Match()
            {
                BoardNumber = 1,
                BoardTypeId = 1,
                PlayerAid = player1,
                PlayerBid = player2,
                RoundId = 1,
                Status = "Pending"
            };

            var match2 = new Match()
            {
                BoardNumber = 2,
                BoardTypeId = 1,
                PlayerAid = player3,
                PlayerBid = player4,
                RoundId = 1,
                Status = "Pending"
            };

            var match3 = new Match()
            {
                BoardNumber = 3,
                BoardTypeId = 2,
                PlayerAid = player5,
                PlayerBid = player6,
                RoundId = 1,
                Status = "Pending"
            };

            var matchesInRound = new List<Match>()
            {
                match1,
                match2,
                match3
            };
            matchesInRound = PairingManager.ResolveHorrorMatches(1, matchesInRound, _dataRepository);

            // Assert number of matches
            Assert.NotEmpty(matchesInRound);
            Assert.Equal(3, matchesInRound.Count);

            var playerProfiles = new Dictionary<int, Tuple<int?, int?>>();
            foreach (var utt in _dataRepository.TournamentUsers.Where(u => u.TournamentId == 1))
            {
                playerProfiles.Add(utt.UserId, new Tuple<int?, int?>(utt.BlockId, utt.FactionId));
            }

            // Assert pairings sanity by Faction
            if (matchesInRound[0].PlayerBid.HasValue)
            {
                Assert.NotEqual(playerProfiles[matchesInRound[0].PlayerAid].Item2, playerProfiles[matchesInRound[0].PlayerBid.Value].Item2);
            }
            if (matchesInRound[1].PlayerBid.HasValue)
            {
                Assert.NotEqual(playerProfiles[matchesInRound[1].PlayerAid].Item2, playerProfiles[matchesInRound[1].PlayerBid.Value].Item2);
            }
            if (matchesInRound[2].PlayerBid.HasValue)
            {
                Assert.NotEqual(playerProfiles[matchesInRound[2].PlayerAid].Item2, playerProfiles[matchesInRound[2].PlayerBid.Value].Item2);
            }
        }

        [Fact]
        public void RandomizeFirstRoundTest()
        {
            PrepareContextData(true);

            var tournamentId = _dataRepository.Tournaments.FirstOrDefault().Id;
            var pairingsAssigned = PairingManager.AssignPlayersForFirstRound(tournamentId, _dataRepository);

            Assert.True(pairingsAssigned);

            var matchesInRound = _dataRepository.Matches.Where(m => m.RoundId == _dataRepository.Rounds.FirstOrDefault().Id).ToList();

            // Assert number of matches
            Assert.NotEmpty(matchesInRound);
            Assert.Equal(3, matchesInRound.Count);

            var playerProfiles = new Dictionary<int, Tuple<int?, int?>>();
            foreach(var utt in _dataRepository.TournamentUsers.Where(u => u.TournamentId == tournamentId))
            {
                playerProfiles.Add(utt.UserId, new Tuple<int?, int?>(utt.BlockId, utt.FactionId));
            }

            // Assert pairings sanity by Faction
            Assert.NotEqual(playerProfiles[matchesInRound[0].PlayerAid].Item2, playerProfiles[matchesInRound[0].PlayerBid.Value].Item2);
            Assert.NotEqual(playerProfiles[matchesInRound[1].PlayerAid].Item2, playerProfiles[matchesInRound[1].PlayerBid.Value].Item2);
            Assert.NotEqual(playerProfiles[matchesInRound[2].PlayerAid].Item2, playerProfiles[matchesInRound[2].PlayerBid.Value].Item2);

            // Assert boards assingment
            Assert.NotEqual(matchesInRound[0].BoardNumber, matchesInRound[1].BoardNumber);
            Assert.NotEqual(matchesInRound[1].BoardNumber, matchesInRound[2].BoardNumber);
            Assert.NotEqual(matchesInRound[2].BoardNumber, matchesInRound[0].BoardNumber);
        }

        [Fact]
        public void RandomizeTwoRoundsTest()
        {
            PrepareContextData();

            var tournamentId = _dataRepository.Tournaments.FirstOrDefault().Id;
            var firstRoundAssigned = PairingManager.AssignPlayersForFirstRound(tournamentId, _dataRepository);

            var players = _dataRepository.TournamentUsers.Where(tu => tu.TournamentId == tournamentId).Select(tu => tu.UserId).ToList();

            Assert.True(firstRoundAssigned);

            var matchesInRound = _dataRepository.Matches.Where(m => m.RoundId == _dataRepository.Rounds.Max(r => r.Id)).ToList();

            // Assert number of matches
            Assert.NotEmpty(matchesInRound);
            Assert.Equal(3, matchesInRound.Count);

            // Provide some points
            matchesInRound[0].Bpa = 10;
            matchesInRound[0].Bpb = 8;
            matchesInRound[0].Spa = 1000;
            matchesInRound[0].Spb = 800;
            matchesInRound[0].SoSa = 7;
            matchesInRound[0].SoSb = 8;

            matchesInRound[1].Bpa = 5;
            matchesInRound[1].Bpb = 12;
            matchesInRound[1].Spa = 500;
            matchesInRound[1].Spb = 1200;
            matchesInRound[1].SoSa = 3;
            matchesInRound[1].SoSb = 5;

            matchesInRound[2].Bpa = 0;
            matchesInRound[2].Bpb = 20;
            matchesInRound[2].Spa = 100;
            matchesInRound[2].Spb = 2200;
            matchesInRound[2].SoSa = 1;
            matchesInRound[2].SoSb = 1;

            var secondRoundAssigned = PairingManager.AssignPairsForNewRound(tournamentId, _dataRepository);

            Assert.True(secondRoundAssigned);

            matchesInRound = _dataRepository.Matches.Where(m => m.RoundId == _dataRepository.Rounds.Max(r => r.Id)).ToList();

            // Assert round players exist in repo
            foreach (var match in matchesInRound)
            {
                Assert.Contains(match.PlayerAid, players);
                if (match.PlayerBid.HasValue)
                {
                    Assert.Contains(match.PlayerBid.Value, players);
                }
            }

            // Assert number of matches
            Assert.NotEmpty(matchesInRound);
            Assert.Equal(3, matchesInRound.Count);

            // Assert boards assingment
            Assert.NotEqual(matchesInRound[0].BoardNumber, matchesInRound[1].BoardNumber);
            Assert.NotEqual(matchesInRound[1].BoardNumber, matchesInRound[2].BoardNumber);
            Assert.NotEqual(matchesInRound[2].BoardNumber, matchesInRound[0].BoardNumber);
        }

        [Fact]
        public void RandomizeThreeRoundsTournamentTest()
        {
            PrepareContextData();

            var tournamentId = _dataRepository.Tournaments.FirstOrDefault().Id;
            var firstRoundAssigned = PairingManager.AssignPlayersForFirstRound(tournamentId, _dataRepository);

            var players = _dataRepository.TournamentUsers.Where(tu => tu.TournamentId == tournamentId).Select(tu => tu.UserId).ToList();

            Assert.True(firstRoundAssigned);

            var matchesInRound = _dataRepository.Matches.Where(m => m.RoundId == _dataRepository.Rounds.Max(r => r.Id)).ToList();

            // Assert number of matches
            Assert.NotEmpty(matchesInRound);
            Assert.Equal(3, matchesInRound.Count);

            // Provide some points
            matchesInRound[0].Bpa = 10;
            matchesInRound[0].Bpb = 8;
            matchesInRound[0].Spa = 1000;
            matchesInRound[0].Spb = 800;
            matchesInRound[0].SoSa = 7;
            matchesInRound[0].SoSb = 8;

            matchesInRound[1].Bpa = 5;
            matchesInRound[1].Bpb = 12;
            matchesInRound[1].Spa = 500;
            matchesInRound[1].Spb = 1200;
            matchesInRound[1].SoSa = 3;
            matchesInRound[1].SoSb = 5;

            matchesInRound[2].Bpa = 0;
            matchesInRound[2].Bpb = 20;
            matchesInRound[2].Spa = 100;
            matchesInRound[2].Spb = 2200;
            matchesInRound[2].SoSa = 1;
            matchesInRound[2].SoSb = 1;

            var secondRoundAssigned = PairingManager.AssignPairsForNewRound(tournamentId, _dataRepository);

            Assert.True(secondRoundAssigned);

            matchesInRound = _dataRepository.Matches.Where(m => m.RoundId == _dataRepository.Rounds.Max(r => r.Id)).ToList();

            // Assert number of matches
            Assert.NotEmpty(matchesInRound);
            Assert.Equal(3, matchesInRound.Count);

            // Provide some points
            matchesInRound[0].Bpa = 20;
            matchesInRound[0].Bpb = 0;
            matchesInRound[0].Spa = 2100;
            matchesInRound[0].Spb = 400;
            matchesInRound[0].SoSa = 7;
            matchesInRound[0].SoSb = 8;

            matchesInRound[1].Bpa = 10;
            matchesInRound[1].Bpb = 10;
            matchesInRound[1].Spa = 1000;
            matchesInRound[1].Spb = 1200;
            matchesInRound[1].SoSa = 3;
            matchesInRound[1].SoSb = 5;

            matchesInRound[2].Bpa = 15;
            matchesInRound[2].Bpb = 3;
            matchesInRound[2].Spa = 1500;
            matchesInRound[2].Spb = 500;
            matchesInRound[2].SoSa = 1;
            matchesInRound[2].SoSb = 1;

            var thirdRoundAssigned = PairingManager.AssignPairsForNewRound(tournamentId, _dataRepository);

            Assert.True(thirdRoundAssigned);

            matchesInRound = _dataRepository.Matches.Where(m => m.RoundId == _dataRepository.Rounds.Max(r => r.Id)).ToList();

            // Assert round players exist in repo
            foreach (var match in matchesInRound)
            {
                Assert.Contains(match.PlayerAid, players);
                if (match.PlayerBid.HasValue)
                {
                    Assert.Contains(match.PlayerBid.Value, players);
                }
            }

            // Assert number of matches
            Assert.NotEmpty(matchesInRound);
            Assert.Equal(3, matchesInRound.Count);

            // Assert boards assingment
            Assert.NotEqual(matchesInRound[0].BoardNumber, matchesInRound[1].BoardNumber);
            Assert.NotEqual(matchesInRound[1].BoardNumber, matchesInRound[2].BoardNumber);
            Assert.NotEqual(matchesInRound[2].BoardNumber, matchesInRound[0].BoardNumber);

            // Assert players scores are meaningfull
            var playersScoresSorted = PairingManager.CalculatePlayersScores(_dataRepository.Tournaments.FirstOrDefault());

            Assert.NotEmpty(playersScoresSorted);
            Assert.Equal(5, playersScoresSorted.Count);
            Assert.True(playersScoresSorted[0].TotalBigPoints >= playersScoresSorted[1].TotalBigPoints);
            Assert.True(playersScoresSorted[1].TotalBigPoints >= playersScoresSorted[2].TotalBigPoints);
            Assert.True(playersScoresSorted[2].TotalBigPoints >= playersScoresSorted[3].TotalBigPoints);
            Assert.True(playersScoresSorted[3].TotalBigPoints >= playersScoresSorted[4].TotalBigPoints);
        }

        public void Dispose()
        {
            _fakeContext.Database.EnsureDeleted();
        }
    }
}
