﻿using DustTournamentKeeper.Infrastructure;
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

            if (gameId == 0)
            {
                return RedirectToAction("Index", "Home");
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

            var currentUserName = User?.Identity?.Name;
            User user = !string.IsNullOrEmpty(currentUserName) ? await _userManager.FindByNameAsync(currentUserName) : null;
            int userId = user?.Id ?? 0;

            var tournamentViewModel = new TournamentViewModel(tournament, userId)
            {
                FinishAvailable =
                (tournament.OrganizerId == userId
                    || await _userManager.IsInRoleAsync(user, nameof(RoleEnum.Administrator)))
                && tournament.Status == nameof(TournamentStatus.Ongoing)
            };

            return View("Details", tournamentViewModel);
        }

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

            var tournamentViewModel = new TournamentViewModel(tournament, user.Id);
            PrepareTournamentViewModel(tournamentViewModel, tournament);

            return View(tournamentViewModel);
        }

        [HttpPost]
        public IActionResult Upsert(TournamentViewModel tournamentViewModel)
        {
            Tournament tournament = null;
            Tournament oldTournament = null;
            List<TournamentUser> tournamentUsers = null;
            List<TournamentUser> oldTournamentUsers = null;

            if (ModelState.IsValid)
            {
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
                    GameId = tournamentViewModel.GameId
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
                            PlayerBid = matchViewModel.PlayerBid
                        };
                        round.Matches.Add(match);
                        if (matchViewModel.Id > 0)
                        {
                            _repository.Update(_repository.Matches.FirstOrDefault(m => m.Id == matchViewModel.Id), match);
                        }

                        var tournamentUserA = tournamentUsers.FirstOrDefault(tu => tu.UserId == matchViewModel.PlayerAid);
                        var tournamentUserB = tournamentUsers.FirstOrDefault(tu => tu.UserId == matchViewModel.PlayerBid);

                        if (tournamentUserA != null)
                        {
                            tournamentUserA.Bp = tournamentUserA.Bp.GetValueOrDefault() + matchViewModel.Bpa;
                            tournamentUserA.Sp = tournamentUserA.Sp.GetValueOrDefault() + matchViewModel.Spa;
                            tournamentUserA.SoS = tournamentUserA.SoS.GetValueOrDefault() + matchViewModel.SoSa;

                            _repository.Update(oldTournamentUsers.FirstOrDefault(tu => tu.Id == tournamentUserA.Id), tournamentUserA);
                        }

                        if (tournamentUserB != null)
                        {
                            tournamentUserB.Bp = tournamentUserB.Bp.GetValueOrDefault() + matchViewModel.Bpb;
                            tournamentUserB.Sp = tournamentUserB.Sp.GetValueOrDefault() + matchViewModel.Spb;
                            tournamentUserB.SoS = tournamentUserB.SoS.GetValueOrDefault() + matchViewModel.SoSb;

                            _repository.Update(oldTournamentUsers.FirstOrDefault(tu => tu.Id == tournamentUserB.Id), tournamentUserB);
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

                foreach (var tournamentBoardType in tournament.TournamentBoardTypes)
                {
                    _repository.Delete(tournamentBoardType);
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

                return RedirectToAction("Details", new { id = tournament.Id });
            }
            else
            {
                PrepareTournamentViewModel(tournamentViewModel, tournament);
                return View(tournamentViewModel);
            }
        }

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

        public IActionResult Register(int tournamentId, int userId)
        {
            var tournament = _repository.Tournaments
                .Include(t => t.TournamentUsers)
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

            if (!tournament.TournamentUsers.Any(utt => utt.UserId == user.Id))
            {
                var blocks = _repository.Blocks.Where(b => b.GameId == tournament.GameId).ToList();
                var factions = _repository.Factions.Where(f => f.GameId == tournament.GameId).ToList();

                var registerToTournamentViewModel = new RegisterToTournamentViewModel()
                {
                    TournamentId = tournamentId,
                    TournamentTitle = tournament.Title,
                    UserId = userId,
                    UserName = user.UserName,
                    BlocksAvailable = blocks.Select(b => new SelectListItem
                        {
                            Text = b.Name,
                            Value = b.Id.ToString()
                        }).ToList(),
                    FactionsAvailable = factions.Select(f => new SelectListItem
                        {
                            Text  = f.Name,
                            Value = f.Id.ToString()
                        }).ToList()
                };

                return View(registerToTournamentViewModel);
            }

            return RedirectToAction("Details", new { id = tournamentId });
        }

        [HttpPost]
        public IActionResult Register(RegisterToTournamentViewModel reg)
        {
            _repository.Add(new TournamentUser
            {
                TournamentId = reg.TournamentId,
                UserId = reg.UserId,
                BlockId = reg.BlockId,
                FactionId = reg.FactionId
            });

            return RedirectToAction("Details", new { id = reg.TournamentId });
        }

        public IActionResult Unregister(int tournamentId, int userId)
        {
            var tournament = _repository.Tournaments
                .Include(t => t.TournamentUsers)
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

            var tournamentUser = _repository.TournamentUsers.FirstOrDefault(tu => tu.TournamentId == tournamentId && tu.UserId == userId);

            if (tournamentUser != null)
            {
                _repository.Delete(tournamentUser);
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

        private void PrepareTournamentViewModel(TournamentViewModel tournamentViewModel, Tournament tournament)
        {
            tournamentViewModel.Boards = _repository.BoardTypes.ToList();
            tournamentViewModel.BoardsSelection = _repository.BoardTypes
                .Select(b => new BoardSelectionFilter()
                {
                    Id = b.Id,
                    Name = b.Name,
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