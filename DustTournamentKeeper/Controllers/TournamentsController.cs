using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DustTournamentKeeper.Controllers
{
    public class TournamentsController : Controller
    {
        private readonly ITournamentRepository _repository;
        private readonly IStringLocalizer<TournamentsController> _localizer;

        public TournamentsController(ITournamentRepository repository, IStringLocalizer<TournamentsController> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public IActionResult Index(int gameId)
        {
            var gameIdInSession = HttpContext.Session.GetInt32("GameSystemId");

            if (gameId > 0 && gameIdInSession != null && gameIdInSession.Value != gameId)
            {
                HttpContext.Session.SetInt32("GameSystemId", gameId);
            }
            else if (gameIdInSession.HasValue && gameIdInSession.Value > 0)
            {
                gameId = gameIdInSession.Value;
            }
            else
            {
                HttpContext.Session.SetInt32("GameSystemId", gameId);
            }

            return View(_repository.Tournaments
                .Include(t => t.ClubNavigation)
                .Include(t => t.Organizer)
                .Where(t => t.GameId == gameId)
                .ToList());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = _repository.Tournaments
                .Include(t => t.ClubNavigation)
                .Include(t => t.Organizer)
                .Include(t => t.BoardTypeToTournament)
                .Include(t => t.Round)
                .Include(t => t.UserToTournament)
                .FirstOrDefault(t => t.Id == id);

            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult RegisterUserToTournament(int tournamentId, int userId)
        {
            var tournament = _repository.Tournaments
                .Include(t => t.UserToTournament)
                .FirstOrDefault(t => t.Id == tournamentId);
            if (tournament == null)
            {
                return NotFound();
            }

            var user = _repository.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            if (!tournament.UserToTournament.Any(utt => utt.UserId == userId))
            {
                _repository.Add(new UserToTournament()
                    {
                        TournamentId = tournamentId,
                        UserId = userId
                    }
                );
            }

            return Details(tournamentId);
        }

        public IActionResult AssignPairsForFirstRound(int tournamentId)
        {
            var pairingSuccssfull = PairingManager.AssignPlayersForFirstRound(tournamentId, _repository);

            return pairingSuccssfull ? Details(tournamentId) : View("Error");
        }

        public IActionResult AssignPairsForNewRound(int tournamentId)
        {
            // Prepare tournament data
            var tournament = _repository.Tournaments
                .Include(t => t.UserToTournament)
                .Include(t => t.Round)
                .FirstOrDefault(t => t.Id == tournamentId);
            if (tournament == null)
            {
                return NotFound();
            }

            // Create new round
            var round = new Round()
            {
                TournamentId = tournament.Id,
                Number = tournament.Round.Count + 1
            };
            _repository.Add(round);

            // Calculate player scores so far
            var playerScores = new List<PlayersTournamentScore>();
            foreach (var player in tournament.UserToTournament)
            {
                var score = new PlayersTournamentScore(player);

                var playerMatches = tournament.Round
                    .SelectMany(r => r.Match.Where(m => m.PlayerAid == player.Id || m.PlayerBid == player.Id));

                foreach (var match in playerMatches)
                {
                    var playerA = tournament.UserToTournament.FirstOrDefault(u => u.UserId == match.PlayerAid);
                    if (match.PlayerAid != player.UserId && !score.Opponents.Contains(playerA))
                    {
                        score.Opponents.Add(playerA);
                        score.TotalBigPoints += match.Bpa.Value;
                        score.TotalSmallPoints += match.Spa.Value;
                        score.TotalSoS += match.SoSa.Value;
                    }

                    var playerB = tournament.UserToTournament.FirstOrDefault(u => u.UserId == match.PlayerBid);
                    if (match.PlayerBid != player.UserId && !score.Opponents.Contains(playerB))
                    {
                        score.Opponents.Add(playerB);
                        score.TotalBigPoints += match.Bpb.Value;
                        score.TotalSmallPoints += match.Spb.Value;
                        score.TotalSoS += match.SoSb.Value;
                    }

                    if (match.PlayerAid == player.UserId && match.PlayerBid == null)
                    {
                        score.HadBye = true;
                    }

                    if (!score.Boards.Contains(match.BoardType))
                    {
                        score.Boards.Add(match.BoardType);
                    }
                }

                playerScores.Add(score);
            }

            // Sort players by their score - BP, SP, SoS, Bye
            var playerScoresSorted = playerScores.OrderBy(ps => ps.TotalBigPoints)
                .ThenBy(ps => ps.TotalSmallPoints)
                .ThenBy(ps => ps.TotalSoS)
                .ThenBy(ps => ps.HadBye)
                .ToList();

            // Assign pairs and boards
            var pairings = new List<Tuple<int, int, int>>();
            var availableBoards = tournament.BoardTypeToTournament.ToList();
            for (int i = 0; playerScoresSorted.Count > 1; )
            {
                BoardTypeToTournament chosenBoard = null;

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
                    chosenBoard = availableBoards[0];
                }
                availableBoards.Remove(chosenBoard);


                pairings.Add(Tuple.Create(playerScoresSorted[i].Player.Id,
                    playerScoresSorted[i+1].Player.Id,
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
                    Status = _localizer["RoundPending"]
                };
                _repository.Add(match);
            }

            return Details(tournamentId);
        }
    }
}