using System;
using System.Collections.Generic;
using System.Linq;
using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using DustTournamentKeeper.ViewModels;
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
                .Include(t => t.Round).ThenInclude(r => r.Match).ThenInclude(m => m.BoardType)
                .Include(t => t.UserToTournament).ThenInclude(u => u.User)
                .Include(t => t.UserToTournament).ThenInclude(u => u.Block)
                .Include(t => t.UserToTournament).ThenInclude(u => u.Faction)
                .FirstOrDefault(t => t.Id == id);

            if (tournament == null)
            {
                return NotFound();
            }

            return View("Details", new TournamentViewModel(tournament));
        }

        public ViewResult Upsert(int id)
        {
            var tournament = _repository.Tournaments
                .Include(t => t.ClubNavigation)
                .Include(t => t.Organizer)
                .Include(t => t.BoardTypeToTournament)
                .Include(t => t.Round).ThenInclude(r => r.Match).ThenInclude(m => m.BoardType)
                .Include(t => t.UserToTournament).ThenInclude(u => u.User)
                .Include(t => t.UserToTournament).ThenInclude(u => u.Block)
                .Include(t => t.UserToTournament).ThenInclude(u => u.Faction)
                .FirstOrDefault(t => t.Id == id);

            return View(tournament ?? new Tournament()
                {
                    GameId = HttpContext.Session.GetInt32("GameSystemId")
                    // OrganizerId = current user
                });
        }

        [HttpPost]
        public IActionResult Upsert(Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                if (tournament.Id > 0)
                {
                    var oldTournament = _repository.Tournaments
                        .Include(t => t.ClubNavigation)
                        .Include(t => t.Organizer)
                        .Include(t => t.BoardTypeToTournament)
                        .Include(t => t.Round).ThenInclude(r => r.Match).ThenInclude(m => m.BoardType)
                        .Include(t => t.UserToTournament).ThenInclude(u => u.User)
                        .Include(t => t.UserToTournament).ThenInclude(u => u.Block)
                        .Include(t => t.UserToTournament).ThenInclude(u => u.Faction)
                        .FirstOrDefault(t => t.Id == tournament.Id);
                    _repository.Update(oldTournament, tournament);
                }
                else
                {
                    _repository.Add(tournament);
                }

                return View("Details", new TournamentViewModel(tournament.Id, _repository));
            }
            else
            {
                return View(tournament);
            }
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

        public IActionResult AssignPairsForFirstRound(int id)
        {
            var pairingSuccssfull = PairingManager.AssignPlayersForFirstRound(id, _repository);

            return pairingSuccssfull ? Details(id) : View("Error");
        }

        public IActionResult AssignPairsForNewRound(int id)
        {
            var pairingSuccssfull = PairingManager.AssignPairsForNewRound(id, _repository);

            return pairingSuccssfull ? Details(id) : View("Error");
        }
    }
}