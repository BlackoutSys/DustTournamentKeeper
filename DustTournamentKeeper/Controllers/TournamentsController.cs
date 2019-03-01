using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using DustTournamentKeeper.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DustTournamentKeeper.Controllers
{
    [Authorize]
    public class TournamentsController : Controller
    {
        private readonly ITournamentRepository _repository;
        private readonly IStringLocalizer<TournamentsController> _localizer;
        private readonly UserManager<User> _userManager;

        public TournamentsController(ITournamentRepository repository, IStringLocalizer<TournamentsController> localizer, UserManager<User> userManager)
        {
            _repository = repository;
            _localizer = localizer;
            _userManager = userManager;
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = _repository.Tournaments
                .Include(t => t.ClubNavigation)
                .Include(t => t.Organizer)
                .Include(t => t.TournamentBoardTypes)
                .Include(t => t.RoundsNavigation).ThenInclude(r => r.Matches).ThenInclude(m => m.BoardType)
                .Include(t => t.TournamentUsers).ThenInclude(u => u.User)
                .Include(t => t.TournamentUsers).ThenInclude(u => u.Block)
                .Include(t => t.TournamentUsers).ThenInclude(u => u.Faction)
                .FirstOrDefault(t => t.Id == id);

            if (tournament == null)
            {
                return NotFound();
            }

            var currentUserName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = await _userManager.FindByNameAsync(currentUserName);

            return View("Details", new TournamentViewModel(tournament, user.Id == tournament.OrganizerId));
        }

        public async Task<ViewResult> Upsert(int? id)
        {
            var tournament = _repository.Tournaments
                .Include(t => t.ClubNavigation)
                .Include(t => t.Organizer)
                .Include(t => t.TournamentBoardTypes)
                .Include(t => t.RoundsNavigation).ThenInclude(r => r.Matches).ThenInclude(m => m.BoardType)
                .Include(t => t.TournamentUsers).ThenInclude(u => u.User)
                .Include(t => t.TournamentUsers).ThenInclude(u => u.Block)
                .Include(t => t.TournamentUsers).ThenInclude(u => u.Faction)
                .FirstOrDefault(t => t.Id == id);

            var currentUserName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = await _userManager.FindByNameAsync(currentUserName);

            return View(tournament ?? new Tournament()
                {
                    GameId = HttpContext.Session.GetInt32("GameSystemId"),
                    OrganizerId = user.Id
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
                        .Include(t => t.TournamentBoardTypes)
                        .Include(t => t.RoundsNavigation).ThenInclude(r => r.Matches).ThenInclude(m => m.BoardType)
                        .Include(t => t.TournamentUsers).ThenInclude(u => u.User)
                        .Include(t => t.TournamentUsers).ThenInclude(u => u.Block)
                        .Include(t => t.TournamentUsers).ThenInclude(u => u.Faction)
                        .FirstOrDefault(t => t.Id == tournament.Id);
                    _repository.Update(oldTournament, tournament);
                }
                else
                {
                    _repository.Add(tournament);
                }

                return RedirectToAction("Details", tournament.Id);
            }
            else
            {
                return View(tournament);
            }
        }

        public IActionResult Finish(int id)
        {
            var tournament = _repository.Tournaments.FirstOrDefault(t => t.Id == id);
            var newTournament = _repository.Tournaments.FirstOrDefault(t => t.Id == id);

            if (tournament != null)
            {
                tournament.Status = "Finished";
                _repository.Update(tournament, newTournament);
                return RedirectToAction("Details", new { id });
            }

            return NotFound();
        }

        public async Task<IActionResult> RegisterUserToTournament(int tournamentId)
        {
            var tournament = _repository.Tournaments
                .Include(t => t.TournamentUsers)
                .FirstOrDefault(t => t.Id == tournamentId);
            if (tournament == null)
            {
                return NotFound();
            }

            var currentUserName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = await _userManager.FindByNameAsync(currentUserName);

            if (!tournament.TournamentUsers.Any(utt => utt.UserId == user.Id))
            {
                _repository.Add(new TournamentUser()
                    {
                        TournamentId = tournamentId,
                        UserId = user.Id
                    }
                );
            }

            return RedirectToAction("Details", new { id = tournamentId });
        }

        public IActionResult AssignPairsForFirstRound(int id)
        {
            var pairingSuccssfull = PairingManager.AssignPlayersForFirstRound(id, _repository);

            if (pairingSuccssfull)
            {
                return RedirectToAction("Details", new { id });
            }

            return View("Error");
        }

        public IActionResult AssignPairsForNewRound(int id)
        {
            var pairingSuccssfull = PairingManager.AssignPairsForNewRound(id, _repository);

            if (pairingSuccssfull)
            {
                return RedirectToAction("Details", new { id });
            }
            else
            {
                return View("Error");
            }
        }
    }
}