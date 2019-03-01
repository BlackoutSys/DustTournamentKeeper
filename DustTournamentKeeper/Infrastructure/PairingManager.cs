using DustTournamentKeeper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DustTournamentKeeper.Infrastructure
{
    public static class PairingManager
    {
        /// <summary>
        /// Assign pairs for the first round of the tournament. 
        /// </summary>
        /// <param name="tournamentId">Tournament ID</param>
        /// <param name="repository">Data repository</param>
        /// <returns>True if assignment successfull. False if tournament could not be found.</returns>
        public static bool AssignPlayersForFirstRound(int tournamentId, ITournamentRepository repository)
        {
            // Prepare tournament data
            var tournament = repository.Tournaments
                .Include(t => t.TournamentUsers).ThenInclude(u => u.User)
                .Include(t => t.RoundsNavigation)
                .Include(t => t.TournamentBoardTypes)
                .FirstOrDefault(t => t.Id == tournamentId);
            if (tournament == null)
            {
                return false;
            }

            // Create new round
            var round = new Round()
            {
                TournamentId = tournament.Id,
                Number = tournament.RoundsNavigation.Count + 1
            };
            repository.Add(round);

            var availablePlayers = tournament.TournamentUsers.ToList();
            var availableBoards = tournament.TournamentBoardTypes.ToList();
            var pairings = new List<Tuple<int, int, TournamentBoardType>>();

            var rand = new Random();
            while (availablePlayers.Count > 1)
            {
                var playerA = availablePlayers[0];
                TournamentUser playerB;

                var playersUniqueLevel3 = availablePlayers
                    .Where(ap => ap.User.City != playerA.User.City
                        && ap.BlockId != playerA.BlockId
                        && ap.FactionId != playerA.FactionId
                        && ap.UserId != playerA.UserId).ToList();

                var playersUniqueLevel2 = availablePlayers
                    .Where(ap => ap.User.City != playerA.User.City
                        && ap.BlockId != playerA.BlockId
                        && ap.UserId != playerA.UserId).ToList();

                var playersUniqueLevel1 = availablePlayers
                    .Where(ap => ap.User.City != playerA.User.City
                        && ap.UserId != playerA.UserId).ToList();

                if (playersUniqueLevel3.Count > 0)
                {
                    int index = rand.Next(0, playersUniqueLevel3.Count);
                    playerB = playersUniqueLevel3[index];
                }
                else if (playersUniqueLevel2.Count > 0)
                {
                    int index = rand.Next(0, playersUniqueLevel2.Count);
                    playerB = playersUniqueLevel2[index];
                }
                else if (playersUniqueLevel1.Count > 0)
                {
                    int index = rand.Next(0, playersUniqueLevel1.Count);
                    playerB = playersUniqueLevel1[index];
                }
                else
                {
                    int index = rand.Next(1, availablePlayers.Count);
                    playerB = availablePlayers[index];
                }

                TournamentBoardType chosenBoard = availableBoards[0];
                availableBoards.Remove(chosenBoard);

                pairings.Add(Tuple.Create(playerA.UserId, playerB.UserId, chosenBoard));

                availablePlayers.Remove(playerA);
                availablePlayers.Remove(playerB);
            }

            // Assign bye
            if (availablePlayers.Count == 1)
            {
                pairings.Add(Tuple.Create(availablePlayers[0].UserId, 0, new TournamentBoardType()));
            }

            // Create Match entities
            var matches = new List<Match>();
            foreach (var pairing in pairings)
            {
                var match = new Match()
                {
                    PlayerAid = pairing.Item1,
                    PlayerBid = pairing.Item2,
                    BoardTypeId = pairing.Item3.BoardTypeId,
                    BoardNumber = pairing.Item3.Number,
                    RoundId = round.Id,
                    Status = "Pending"
                };
                matches.Add(match);
            }

            // Check for horror matches and try to resolve them
            matches = ResolveHorrorMatches(tournamentId, matches, repository);

            matches.ForEach(m => repository.Add(m));

            return true;
        }

        /// <summary>
        /// Assign pairs for next round of the tournament. 
        /// </summary>
        /// <param name="tournamentId">Tournament ID</param>
        /// <param name="repository">Data repository</param>
        /// <returns>True if assignment successfull. False if tournament could not be found.</returns>
        public static bool AssignPairsForNewRound(int tournamentId, ITournamentRepository repository)
        {
            // Prepare tournament data
            var tournament = repository.Tournaments
                .Include(t => t.TournamentUsers)
                .Include(t => t.RoundsNavigation)
                .FirstOrDefault(t => t.Id == tournamentId);
            if (tournament == null)
            {
                return false;
            }

            // Create new round
            var round = new Round()
            {
                TournamentId = tournament.Id,
                Number = tournament.RoundsNavigation.Count + 1
            };
            repository.Add(round);

            // Calculate player scores so far
            var playerScoresSorted = CalculatePlayersScores(tournament);

            // Assign pairs and boards
            var pairings = new List<Tuple<int, int, int>>();
            var availableBoards = tournament.TournamentBoardTypes.ToList();
            for (int i = 0; playerScoresSorted.Count > 1;)
            {
                TournamentBoardType chosenBoard = null;

                var playerA = playerScoresSorted[i];
                var playerB = playerScoresSorted[i + 1];

                var uniqueBoardsA = availableBoards.Where(b =>
                    !playerA.Boards.Contains(b.BoardType));

                var uniqueBoardsB = availableBoards.Where(b =>
                    !playerB.Boards.Contains(b.BoardType));

                var uniqueBoardsIntersection = uniqueBoardsA.Intersect(uniqueBoardsB);

                if (uniqueBoardsIntersection.Any())
                {
                    chosenBoard = uniqueBoardsIntersection.First();
                }
                else
                {
                    var rand = new Random();
                    chosenBoard = availableBoards[rand.Next(0, availableBoards.Count)];
                }
                availableBoards.Remove(chosenBoard);


                pairings.Add(Tuple.Create(playerScoresSorted[i].Player.Id,
                    playerScoresSorted[i + 1].Player.Id,
                    chosenBoard.Id));
                playerScoresSorted.RemoveAt(i);
                playerScoresSorted.RemoveAt(i);
            }

            // Assign bye
            if (playerScoresSorted.Count > 0)
            {
                pairings.Add(Tuple.Create(playerScoresSorted[0].Player.Id, 0, 0));
            }

            // Create Match entities
            foreach (var pairing in pairings)
            {
                var match = new Match()
                {
                    PlayerAid = pairing.Item1,
                    PlayerBid = pairing.Item2,
                    BoardTypeId = pairing.Item3,
                    RoundId = round.Id,
                    Status = "Pending"
                };
                repository.Add(match);
            }

            return true;
        }

        /// <summary>
        /// Calculates players scores so far in given tournament
        /// </summary>
        /// <param name="tournament">Tournament</param>
        /// <returns>List of players scores so far</returns>
        public static List<PlayersTournamentScore> CalculatePlayersScores(Tournament tournament)
        {
            var playerScores = new List<PlayersTournamentScore>();
            foreach (var player in tournament.TournamentUsers)
            {
                var score = new PlayersTournamentScore(player);

                var playerMatches = tournament.RoundsNavigation
                    .SelectMany(r => r.Matches.Where(m => m.PlayerAid == player.Id || m.PlayerBid == player.Id));

                // Check for bye
                if (playerMatches.Any(pm => pm.PlayerAid == player.UserId && pm.PlayerBid == null))
                {
                    score.HadBye = true;
                }

                // Distinct list of boards player had played on
                score.Boards = playerMatches.Select(pm => pm.BoardType).Distinct().ToList();

                // Total score
                foreach (var match in playerMatches)
                {
                    TournamentUser chosenPlayer = null;
                    var playerA = tournament.TournamentUsers.FirstOrDefault(u => u.UserId == match.PlayerAid);
                    var playerB = tournament.TournamentUsers.FirstOrDefault(u => u.UserId == match.PlayerBid);
                    if (match.PlayerAid != player.UserId && !score.Opponents.Contains(playerA))
                    {
                        chosenPlayer = playerA;
                    }
                    else
                    {
                        chosenPlayer = playerB;
                    }

                    score.Opponents.Add(chosenPlayer);
                    score.TotalBigPoints += match.Bpb ?? 0;
                    score.TotalSmallPoints += match.Spb ?? 0;
                    score.TotalSoS += match.SoSb ?? 0;
                }

                score.BonusPoints = player.BonusPoints ?? 0;
                score.PenaltyPoints = player.PenaltyPoints ?? 0;

                playerScores.Add(score);
            }

            // Sort players by their score - BP + Bonus - Penalty, SP, SoS, Bye
            return playerScores.OrderByDescending(ps => ps.TotalBigPoints + ps.BonusPoints - ps.PenaltyPoints)
                .ThenByDescending(ps => ps.TotalSmallPoints)
                .ThenByDescending(ps => ps.TotalSoS)
                .ThenByDescending(ps => ps.HadBye)
                .ToList();
        }

        /// <summary>
        /// Finds horror matches and tries to resolve them by shifting parirings.
        /// </summary>
        /// <param name="tournamentId">Tournament id</param>
        /// <param name="matches">List of proposed pairings</param>
        /// <param name="repository">Data repository</param>
        public static List<Match> ResolveHorrorMatches(int tournamentId, List<Match> matches, ITournamentRepository repository)
        {
            // Prepare player profiles for quick lookups
            var playerProfiles = new Dictionary<int, Tuple<int?, int?>>();
            foreach (var utt in repository.TournamentUsers.Where(u => u.TournamentId == tournamentId))
            {
                playerProfiles.Add(utt.UserId, new Tuple<int?, int?>(utt.BlockId, utt.FactionId));
            }

            // Find any horror matches
            var horrorMatches = new List<Match>();
            foreach (var match in matches)
            {
                if (!match.PlayerBid.HasValue)
                {
                    continue;
                }

                if (playerProfiles[match.PlayerAid].Item1 == playerProfiles[match.PlayerBid.Value].Item1
                    && playerProfiles[match.PlayerAid].Item2 == playerProfiles[match.PlayerBid.Value].Item2)
                {
                    horrorMatches.Add(match);
                }
            }

            // Try to resolve by two levels (by block and faction)
            matches = ResolveHorrorMachtesByTwoLevels(matches, ref horrorMatches, playerProfiles);

            // Try to resolve by one level if any horror matches still remain (just by block)
            matches = ResolveHorrorMachtesByOneLevel(matches, ref horrorMatches, playerProfiles);

            return matches;
        }

        /// <summary>
        /// Tries to resolve them by shifting parirings. Granuality is two levels - block and faction
        /// </summary>
        /// <param name="matches">List of assigned matches</param>
        /// <param name="horrorMatches">List of horrors matches detected</param>
        /// <param name="playerProfiles">Lookup dictionary of players</param>
        private static List<Match> ResolveHorrorMachtesByTwoLevels(List<Match> matches, ref List<Match> horrorMatches, Dictionary<int, Tuple<int?, int?>> playerProfiles)
        {
            var horrorCount = horrorMatches.Count;
            for (var i = 0; i < horrorCount; i++)
            {
                var horrorMatch = horrorMatches[0];

                // Check if horror wasn't already resolved in previous iteration
                if (!matches.Contains(horrorMatch))
                {
                    horrorMatches.Remove(horrorMatch);
                    continue;
                }

                var mashupCandidateMatch = matches.Find(m =>
                        m.PlayerAid != horrorMatch.PlayerAid && m.PlayerBid != horrorMatch.PlayerAid
                        && m.PlayerAid != horrorMatch.PlayerBid && m.PlayerBid != horrorMatch.PlayerBid
                        && (playerProfiles[horrorMatch.PlayerAid].Item1 != playerProfiles[m.PlayerAid].Item1
                            || !m.PlayerBid.HasValue
                            || playerProfiles[horrorMatch.PlayerAid].Item1 != playerProfiles[m.PlayerBid.Value].Item1)
                        && (playerProfiles[horrorMatch.PlayerBid.Value].Item2 != playerProfiles[m.PlayerAid].Item2
                            || !m.PlayerBid.HasValue
                            || playerProfiles[horrorMatch.PlayerBid.Value].Item2 != playerProfiles[m.PlayerBid.Value].Item2)
                    );

                if (mashupCandidateMatch != null)
                {
                    matches.Remove(mashupCandidateMatch);
                    matches.Remove(horrorMatch);
                    horrorMatches.Remove(horrorMatch);

                    int playerA1 = 0, playerA2 = 0;
                    int? playerB1 = 0, playerB2 = 0;

                    if (playerProfiles[horrorMatch.PlayerAid].Item1 != playerProfiles[mashupCandidateMatch.PlayerAid].Item1
                        && playerProfiles[horrorMatch.PlayerAid].Item2 != playerProfiles[mashupCandidateMatch.PlayerAid].Item2
                        && (!mashupCandidateMatch.PlayerBid.HasValue
                            || playerProfiles[horrorMatch.PlayerBid.Value].Item1 != playerProfiles[mashupCandidateMatch.PlayerBid.Value].Item1)
                        && (!mashupCandidateMatch.PlayerBid.HasValue
                            || playerProfiles[horrorMatch.PlayerBid.Value].Item2 != playerProfiles[mashupCandidateMatch.PlayerBid.Value].Item2))
                    {
                        playerA1 = horrorMatch.PlayerAid;
                        playerB1 = mashupCandidateMatch.PlayerAid;
                        playerA2 = horrorMatch.PlayerBid.Value;
                        playerB2 = mashupCandidateMatch.PlayerBid;
                    }
                    else
                    {
                        playerA1 = horrorMatch.PlayerAid;
                        playerB1 = mashupCandidateMatch.PlayerBid;
                        playerA2 = horrorMatch.PlayerBid.Value;
                        playerB2 = mashupCandidateMatch.PlayerAid;
                    }

                    matches.Add(new Match()
                    {
                        BoardNumber = mashupCandidateMatch.BoardNumber,
                        BoardTypeId = mashupCandidateMatch.BoardTypeId,
                        RoundId = mashupCandidateMatch.RoundId,
                        Status = mashupCandidateMatch.Status,
                        PlayerAid = playerA1,
                        PlayerBid = playerB1,
                    });

                    matches.Add(new Match()
                    {
                        BoardNumber = horrorMatch.BoardNumber,
                        BoardTypeId = horrorMatch.BoardTypeId,
                        RoundId = horrorMatch.RoundId,
                        Status = horrorMatch.Status,
                        PlayerAid = playerA2,
                        PlayerBid = playerB2,
                    });
                }
            }

            return matches;
        }

        /// <summary>
        /// Tries to resolve them by shifting parirings. Granuality is one level - block
        /// </summary>
        /// <param name="matches">List of assigned matches</param>
        /// <param name="horrorMatches">List of horrors matches detected</param>
        /// <param name="playerProfiles">Lookup dictionary of players</param>
        private static List<Match> ResolveHorrorMachtesByOneLevel(List<Match> matches, ref List<Match> horrorMatches, Dictionary<int, Tuple<int?, int?>> playerProfiles)
        {
            var horrorCount = horrorMatches.Count;
            for (var i = 0; i < horrorCount; i++)
            {
                var horrorMatch = horrorMatches[0];

                // Check if horror wasn't already resolved in previous iteration
                if (!matches.Contains(horrorMatch))
                {
                    horrorMatches.Remove(horrorMatch);
                    continue;
                }

                var mashupCandidateMatch = matches.Find(m =>
                        m.PlayerAid != horrorMatch.PlayerAid && m.PlayerBid != horrorMatch.PlayerAid
                        && m.PlayerBid != horrorMatch.PlayerAid && m.PlayerBid != horrorMatch.PlayerAid
                        && (playerProfiles[horrorMatch.PlayerAid].Item1 != playerProfiles[m.PlayerAid].Item1
                            || !m.PlayerBid.HasValue
                            || playerProfiles[horrorMatch.PlayerAid].Item1 != playerProfiles[m.PlayerBid.Value].Item1)
                    );

                if (mashupCandidateMatch != null)
                {
                    matches.Remove(mashupCandidateMatch);
                    matches.Remove(horrorMatch);
                    horrorMatches.Remove(horrorMatch);

                    int playerA1 = 0, playerA2 = 0;
                    int? playerB1 = 0, playerB2 = 0;

                    if (playerProfiles[horrorMatch.PlayerAid].Item1 != playerProfiles[mashupCandidateMatch.PlayerAid].Item1
                        && (!mashupCandidateMatch.PlayerBid.HasValue
                            || playerProfiles[horrorMatch.PlayerBid.Value].Item1 != playerProfiles[mashupCandidateMatch.PlayerBid.Value].Item1))
                    {
                        playerA1 = horrorMatch.PlayerAid;
                        playerB1 = mashupCandidateMatch.PlayerAid;
                        playerA2 = horrorMatch.PlayerBid.Value;
                        playerB2 = mashupCandidateMatch.PlayerBid;
                    }
                    else
                    {
                        playerA1 = horrorMatch.PlayerAid;
                        playerB1 = mashupCandidateMatch.PlayerBid;
                        playerA2 = horrorMatch.PlayerBid.Value;
                        playerB2 = mashupCandidateMatch.PlayerAid;
                    }

                    matches.Add(new Match()
                    {
                        BoardNumber = mashupCandidateMatch.BoardNumber,
                        BoardTypeId = mashupCandidateMatch.BoardTypeId,
                        RoundId = mashupCandidateMatch.RoundId,
                        Status = mashupCandidateMatch.Status,
                        PlayerAid = playerA1,
                        PlayerBid = playerB1,
                    });

                    matches.Add(new Match()
                    {
                        BoardNumber = horrorMatch.BoardNumber,
                        BoardTypeId = horrorMatch.BoardTypeId,
                        RoundId = horrorMatch.RoundId,
                        Status = horrorMatch.Status,
                        PlayerAid = playerA2,
                        PlayerBid = playerB2,
                    });
                }
            }

            return matches;
        }
    }
}
