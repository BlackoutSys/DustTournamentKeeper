using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using DustTournamentKeeper.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustTournamentKeeper.Controllers
{
    [Authorize]
    public class TournamentsController : Controller
    {
        private readonly ITournamentRepository _repository;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<TournamentsController> _localizer;

        public TournamentsController(ITournamentRepository repository, IStringLocalizer<TournamentsController> localizer, UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
            _localizer = localizer;
        }

        [AllowAnonymous]
        public IActionResult Index(int gameId)
        {
            var gameIdInSession = HttpContext.Session.GetInt32("GameSystemId");

            if (gameId > 0)
            {
                HttpContext.Session.SetInt32("GameSystemId", gameId);
            }
            else if (gameIdInSession.HasValue && gameIdInSession.Value > 0)
            {
                gameId = gameIdInSession.Value;
            }

            if (gameId == 0)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View(_repository.Tournaments
                .Include(t => t.ClubNavigation)
                .Include(t => t.Organizer)
                .Where(t => t.GameId == gameId)
                .OrderByDescending(t => t.DateStart)
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

            var currentUserName = User?.Identity?.Name;
            User user = !string.IsNullOrEmpty(currentUserName) ? await _userManager.FindByNameAsync(currentUserName) : null;
            int userId = user?.Id ?? 0;

            var tournamentViewModel = new TournamentViewModel(tournament, userId, _localizer)
            {
                FinishAvailable =
                (tournament.OrganizerId == userId
                    || (user != null && await _userManager.IsInRoleAsync(user, nameof(Roles.Administrator))))
                && tournament.Status == nameof(TournamentStatus.Ongoing)
            };

            return View("Details", tournamentViewModel);
        }

        [Authorize(Roles = "Administrator,Organizer")]
        public async Task<ViewResult> Upsert(int? id)
        {
            var tournament = id.HasValue ? _repository.Tournaments
                .Include(t => t.ClubNavigation)
                .Include(t => t.Organizer)
                .Include(t => t.TournamentBoardTypes).ThenInclude(tb => tb.BoardType)
                .Include(t => t.RoundsNavigation).ThenInclude(r => r.Matches).ThenInclude(m => m.BoardType)
                .Include(t => t.TournamentUsers).ThenInclude(u => u.User)
                .Include(t => t.TournamentUsers).ThenInclude(u => u.Block)
                .Include(t => t.TournamentUsers).ThenInclude(u => u.Faction)
                .FirstOrDefault(t => t.Id == id) : new Tournament();

            var currentUserName = User.Identity.Name;
            User user = await _userManager.FindByNameAsync(currentUserName);
            tournament.Organizer = user;

            var tournamentViewModel = new TournamentViewModel(tournament, user.Id, _localizer);
            PrepareViewModel(tournamentViewModel, tournament);

            return View(tournamentViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Organizer")]
        [RequestSizeLimit(52428800)]
        public IActionResult Upsert(TournamentViewModel tournamentViewModel)
        {
            Tournament tournament = null;
            Tournament oldTournament = null;
            List<TournamentUser> tournamentUsers = null;
            List<TournamentUser> oldTournamentUsers = null;

            if (!ModelState.IsValid)
            {
                PrepareViewModel(tournamentViewModel, tournament);
                return View(tournamentViewModel);
            }

            tournament = new Tournament()
            {
                Id = tournamentViewModel.Id,
                DateStart = tournamentViewModel.DateStart,
                DateEnd = tournamentViewModel.DateEnd,
                City = tournamentViewModel.City,
                Address = tournamentViewModel.Address,
                Club = tournamentViewModel.Club,
                ClubId = tournamentViewModel.ClubId,
                Title = tournamentViewModel.Title,
                Slogan = tournamentViewModel.Slogan,
                PlayerLimit = tournamentViewModel.PlayerLimit,
                Status = tournamentViewModel.Status,
                Rounds = tournamentViewModel.Rounds,
                ArmyPoints = tournamentViewModel.ArmyPoints,
                SpecificRules = tournamentViewModel.SpecificRules,
                OrganizerId = tournamentViewModel.OrganizerId,
                Created = tournamentViewModel.Created,
                Fee = tournamentViewModel.Fee,
                FeeCurrency = tournamentViewModel.FeeCurrency,
                Bpwin = tournamentViewModel.Bpwin,
                Bptie = tournamentViewModel.Bptie,
                Bploss = tournamentViewModel.Bploss,
                Country = tournamentViewModel.Country,
                GameId = tournamentViewModel.GameId,
                TieBreaker1 = tournamentViewModel.TieBreaker1,
                TieBreaker2 = tournamentViewModel.TieBreaker2,
                TieBreaker3 = tournamentViewModel.TieBreaker3,
                TieBreaker4 = tournamentViewModel.TieBreaker4
            };

            if (tournamentViewModel.Id > 0)
            {
                oldTournament = _repository.Tournaments
                            .Include(t => t.ClubNavigation)
                            .Include(t => t.Organizer)
                            .Include(t => t.TournamentBoardTypes)
                            .Include(t => t.RoundsNavigation).ThenInclude(r => r.Matches).ThenInclude(m => m.BoardType)
                            .FirstOrDefault(t => t.Id == tournamentViewModel.Id);

                tournamentUsers = _repository.TournamentUsers.Where(tu => tu.TournamentId == tournamentViewModel.Id).ToList();
                oldTournamentUsers = tournamentUsers.ToList();

                foreach (var playerViewModel in tournamentViewModel.PlayersList)
                {
                    var tournamentUser = tournamentUsers.FirstOrDefault(tu => tu.Id == playerViewModel.Id);
                    var oldTournamentUser = oldTournamentUsers.FirstOrDefault(otu => otu.Id == playerViewModel.Id);
                    if (tournamentUser != null && oldTournamentUser != null)
                    {
                        tournamentUser.BonusPoints = playerViewModel.BonusPoints;
                        tournamentUser.PenaltyPoints = playerViewModel.PenaltyPoints;

                        _repository.Update(oldTournamentUser, tournamentUser);
                    }
                }

                foreach (var tournamentUser in tournamentUsers)
                {
                    tournamentUser.Bp = 0;
                    tournamentUser.Sp = 0;
                    tournamentUser.SoS = 0;
                }
            }

            tournament.RoundsNavigation = new List<Round>();
            foreach (var roundViewModel in tournamentViewModel.RoundsList)
            {
                var round = new Round()
                {
                    Id = roundViewModel.Id,
                    TournamentId = tournamentViewModel.Id,
                    Number = roundViewModel.Number,

                };
                round.Matches = new List<Match>();
                foreach (var matchViewModel in roundViewModel.Matches)
                {
                    var match = new Match()
                    {
                        Id = matchViewModel.Id,
                        RoundId = roundViewModel.Id,
                        BoardTypeId = matchViewModel.BoardTypeId,
                        BoardNumber = matchViewModel.BoardNumber,
                        Bpa = matchViewModel.Bpa,
                        Bpb = matchViewModel.Bpb,
                        Spa = matchViewModel.Spa,
                        Spb = matchViewModel.Spb,
                        PlayerAid = matchViewModel.PlayerAid,
                        PlayerBid = matchViewModel.PlayerBid,
                        SoSa = matchViewModel.SoSa,
                        SoSb = matchViewModel.SoSb
                    };
                    round.Matches.Add(match);
                    if (matchViewModel.Id > 0)
                    {
                        _repository.Update(_repository.Matches.FirstOrDefault(m => m.Id == matchViewModel.Id), match);
                    }

                    if (matchViewModel.Bpa == null && matchViewModel.Bpb == null) 
                    {
                        continue;
                    }

                    var tournamentUserA = tournamentUsers.FirstOrDefault(tu => tu.UserId == matchViewModel.PlayerAid);
                    var tournamentUserB = tournamentUsers.FirstOrDefault(tu => tu.UserId == matchViewModel.PlayerBid);

                    if (tournamentUserA != null)
                    {
                        tournamentUserA.Bp = tournamentUserA.Bp.GetValueOrDefault() + matchViewModel.Bpa ?? 0;
                        tournamentUserA.Sp = tournamentUserA.Sp.GetValueOrDefault() + matchViewModel.Spa ?? 0;
                        tournamentUserA.SoS = tournamentUserA.SoS.GetValueOrDefault() + matchViewModel.SoSa ?? 0;

                        _repository.Update(_repository.TournamentUsers.FirstOrDefault(tu => tu.Id == tournamentUserA.Id), tournamentUserA);
                    }

                    if (tournamentUserB != null)
                    {
                        tournamentUserB.Bp = tournamentUserB.Bp.GetValueOrDefault() + matchViewModel.Bpb ?? 0;
                        tournamentUserB.Sp = tournamentUserB.Sp.GetValueOrDefault() + matchViewModel.Spb ?? 0;
                        tournamentUserB.SoS = tournamentUserB.SoS.GetValueOrDefault() + matchViewModel.SoSb ?? 0;

                        _repository.Update(_repository.TournamentUsers.FirstOrDefault(tu => tu.Id == tournamentUserB.Id), tournamentUserB);
                    }
                }

                tournament.RoundsNavigation.Add(round);
            }

            if (tournamentViewModel.Id > 0)
            {
                _repository.Update(oldTournament, tournament);
            }
            else
            {
                _repository.Add(tournament);
            }

            if (tournament.Status == nameof(TournamentStatus.Draft) || tournament.Status == nameof(TournamentStatus.Pending))
            {
                if (oldTournament != null)
                {
                    int count = oldTournament.TournamentBoardTypes.Count;
                    for (int i = 0; i < count; i++)
                    {
                        _repository.Delete(oldTournament.TournamentBoardTypes[0]);
                    }
                }

                int counter = 1;
                foreach (var boardSelection in tournamentViewModel.BoardsSelection)
                {
                    for (int i = 0; i < boardSelection.Count; i++)
                    {
                        _repository.Add(new TournamentBoardType()
                        {
                            BoardTypeId = boardSelection.Id,
                            Number = counter,
                            TournamentId = tournament.Id
                        });
                        counter++;
                    }
                }
            }
            
            return RedirectToAction("Details", new { id = tournament.Id });
        }

        [Authorize(Roles = "Administrator,Organizer")]
        public IActionResult Begin(int id)
        {
            var tournament = _repository.Tournaments.FirstOrDefault(t => t.Id == id);
            var newTournament = _repository.Tournaments.FirstOrDefault(t => t.Id == id);

            if (tournament != null)
            {
                newTournament.Status = nameof(TournamentStatus.Ongoing);
                _repository.Update(tournament, newTournament);
                return RedirectToAction("Details", new { id });
            }

            return NotFound();
        }

        [Authorize(Roles = "Administrator,Organizer")]
        public IActionResult Finish(int id)
        {
            var tournament = _repository.Tournaments.FirstOrDefault(t => t.Id == id);
            var newTournament = _repository.Tournaments.FirstOrDefault(t => t.Id == id);

            if (tournament != null)
            {
                newTournament.Status = nameof(TournamentStatus.Finished);
                _repository.Update(tournament, newTournament);
                return RedirectToAction("Details", new { id });
            }

            return NotFound();
        }

        public IActionResult Register(int tournamentId, string user)
        {
            var tournament = _repository.Tournaments
                .Include(t => t.TournamentUsers)
                .FirstOrDefault(t => t.Id == tournamentId);
            if (tournament == null)
            {
                return NotFound();
            }

            User userObj;
            if (int.TryParse(user, out int userId))
            {
                userObj = _repository.Users.FirstOrDefault(u => u.Id == userId);
            }
            else
            {
                userObj = _repository.Users.FirstOrDefault(u => u.UserName == user);
            }

            if (userObj == null)
            {
                return NotFound();
            }

            return RegisterInternal(tournament, userObj);
        }
        
        [HttpPost]
        public IActionResult Register(RegisterToTournamentViewModel reg)
        {
            var tournamentUser = _repository.TournamentUsers.FirstOrDefault(x => x.TournamentId == reg.TournamentId && x.UserId == reg.UserId);
            var newTournamentUser = new TournamentUser
            {
                TournamentId = reg.TournamentId,
                UserId = reg.UserId,
                BlockId = reg.BlockId,
                FactionId = reg.FactionId
            };

            if (tournamentUser == null)
            {
                _repository.Add(newTournamentUser);
            }
            else 
            {
                _repository.Update(tournamentUser, newTournamentUser, true);
            }

            return RedirectToAction("Details", new { id = reg.TournamentId });
        }

        private IActionResult RegisterInternal(Tournament tournament, User user)
        {
            var blocks = _repository.Blocks.Where(b => b.GameId == tournament.GameId).ToList();
            var factions = _repository.Factions.Where(f => f.GameId == tournament.GameId).ToList();

            var registerToTournamentViewModel = new RegisterToTournamentViewModel()
            {
                TournamentId = tournament.Id,
                TournamentTitle = tournament.Title,
                UserId = user.Id,
                UserName = user.UserName,
                BlocksAvailable = blocks.Select(b => new SelectListItem
                {
                    Text = b.Name,
                    Value = b.Id.ToString()
                }).ToList(),
                FactionsAvailable = factions.Select(f => new SelectListItem
                {
                    Text = f.Name,
                    Value = f.Id.ToString()
                }).ToList()
            };

            return View(registerToTournamentViewModel);
        }

        public IActionResult Unregister(int tournamentId, int userId)
        {
            var tournament = _repository.Tournaments
                .Include(t => t.TournamentUsers)
                .Include(t => t.RoundsNavigation).ThenInclude(r => r.Matches)
                .FirstOrDefault(t => t.Id == tournamentId);
            if (tournament == null)
            {
                return NotFound();
            }

            User user = _repository.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            if (tournament.RoundsNavigation.FirstOrDefault(r =>
                 r.Matches.FirstOrDefault(m =>
                     m.PlayerAid == userId || m.PlayerBid == userId) != null) != null)
            {
                return View("Error", new ErrorViewModel { ErrorText = _localizer["CannotKick"] });
            }

            var tournamentUser = _repository.TournamentUsers.FirstOrDefault(tu => tu.TournamentId == tournamentId && tu.UserId == userId);

            if (tournamentUser != null)
            {
                _repository.Delete(tournamentUser);
            }

            return RedirectToAction("Details", new { id = tournamentId });
        }

        [Authorize(Roles = "Administrator,Organizer")]
        public IActionResult AssignPairsForFirstRound(int id)
        {
            var pairingSuccssfull = PairingManager.AssignPlayersForFirstRound(id, _repository);

            if (pairingSuccssfull)
            {
                return RedirectToAction("Details", new { id });
            }

            return View("Error", new ErrorViewModel { ErrorText = _localizer["AssignmentError"] });
        }

        [Authorize(Roles = "Administrator,Organizer")]
        public IActionResult AssignPairsForNewRound(int id)
        {
            var pairingSuccssfull = PairingManager.AssignPairsForNewRound(id, _repository);

            if (pairingSuccssfull)
            {
                return RedirectToAction("Details", new { id });
            }
            else
            {
                return View("Error", new ErrorViewModel { ErrorText = _localizer["AssignmentError"] });
            }
        }

        [Authorize(Roles = "Administrator,Organizer")]
        public IActionResult DropMostRecentRound(int id)
        {
            var tournament = _repository.Tournaments
                .Include(t => t.RoundsNavigation)
                .ThenInclude(r => r.Matches)
                .FirstOrDefault(t => t.Id == id);

            if (tournament == null)
            {
                return NotFound("Tournament was not found");
            }

            var lastRound = tournament.RoundsNavigation.Last();

            if (lastRound.Matches.Any(m => m.Bpa != null))
            {
                return View("Error", new ErrorViewModel { ErrorText = _localizer["DropRoundError"] });
            }

            var count = lastRound.Matches.Count;
            for (int i = 0; i < count; i++)
            {
                _repository.Delete(lastRound.Matches[0]);
            }

            _repository.Delete(lastRound);

            return RedirectToAction("Details", new { id });
        }

        [AllowAnonymous]
        public IActionResult ShowPairings(int id)
        {
            var tournament = _repository.Tournaments
                .Include(t => t.RoundsNavigation).ThenInclude(r => r.Matches).ThenInclude(m => m.PlayerA)
                .Include(t => t.RoundsNavigation).ThenInclude(r => r.Matches).ThenInclude(m => m.PlayerB)
                .Include(t => t.RoundsNavigation).ThenInclude(r => r.Matches).ThenInclude(m => m.BoardType)
                .FirstOrDefault(t => t.Id == id);

            if (tournament == null)
            {
                return View("Error", new ErrorViewModel { ErrorText = _localizer["NoTournament"] });
            }

            if (tournament.RoundsNavigation.Count == 0)
            {
                return View("Error", new ErrorViewModel { ErrorText = _localizer["NoPairings"] });
            }

            var lastRound = tournament.RoundsNavigation.Last();

            var viewModel = new TournamentPairingViewModel
            {
                TournamentId = tournament.Id,
                TournamentTitle = tournament.Title,
                RoundNumber = lastRound.Number,
                Matches = lastRound.Matches
            };

            return View("Pairing", viewModel);
        }

        private void PrepareViewModel(TournamentViewModel tournamentViewModel, Tournament tournament)
        {
            tournamentViewModel.Boards = _repository.BoardTypes.ToList();
            tournamentViewModel.BoardsSelection = _repository.BoardTypes
                .Select(b => new BoardSelectionFilter()
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    Count = 0
                }).ToList();

            tournamentViewModel.ClubsAvailable = _repository.Clubs.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            tournamentViewModel.GamesAvailable = _repository.Games.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            tournamentViewModel.StatusesAvailable = new List<SelectListItem>();
            foreach (TournamentStatus status in Enum.GetValues(typeof(TournamentStatus)))
            {
                tournamentViewModel.StatusesAvailable.Add(new SelectListItem()
                {
                    Text = status.ToString(),
                    Value = status.ToString()
                });
            }

            if (tournament != null)
            {
                foreach (var tournamentBoardType in tournament.TournamentBoardTypes)
                {
                    tournamentViewModel.BoardsSelection.First(b => b.Id == tournamentBoardType.BoardTypeId).Count++;
                }

                tournamentViewModel.PlayersAvailable = tournament.TournamentUsers
                    .Select(u => new SelectListItem
                    {
                        Text = u.User.UserName,
                        Value = u.UserId.ToString()
                    }).ToList();

                tournamentViewModel.BoardsAvailable = tournament.TournamentBoardTypes
                    .Select(t => new SelectListItem
                    {
                        Text = t.BoardType.Name,
                        Value = t.BoardTypeId.ToString()
                    }).ToList();
            }
        }
    }
}