using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DustTournamentKeeper.Controllers
{
    public class RankingsController : Controller
    {
        private readonly ITournamentRepository _repository;

        public RankingsController(ITournamentRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index(int gameId, DateTime? dateFrom, DateTime? dateTo,
            string country, string city, int? clubId, string clubName,
            int? bigPointsMin, int? bigPointsMax, int? smallPointsMin, int? smallPointsMax)
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

            var filter = new RankingFilter
            {
                DateFrom = dateFrom,
                DateTo = dateTo,
                Country = country,
                City = city,
                ClubId = clubId,
                ClubName = clubName,
                BigPointsMin = bigPointsMin,
                BigPointsMax = bigPointsMax,
                SmallPointsMin = smallPointsMin,
                SmallPointsMax = smallPointsMax
            };

            var tournaments = _repository.Tournaments
                .Include(t => t.ClubNavigation)
                .Include(t => t.TournamentUsers).ThenInclude(tu => tu.User)
                .Where(t => 
                    t.GameId == gameId
                    && t.Status == nameof(TournamentStatus.Finished)
                );

            if (filter.DateFrom.HasValue)
            {
                tournaments = tournaments.Where(t => t.DateStart >= filter.DateFrom.Value);
            }
            if (filter.DateTo.HasValue)
            {
                tournaments = tournaments.Where(t => t.DateEnd <= filter.DateTo.Value);
            }

            if (filter.ClubId.HasValue && filter.ClubId.Value > 0)
            {
                tournaments = tournaments.Where(t => t.ClubId == filter.ClubId);
            } else if (!string.IsNullOrWhiteSpace(filter.ClubName))
            {
                tournaments = tournaments.Where(t => t.Club.ToLower() == filter.ClubName.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(filter.Country))
            {
                tournaments = tournaments.Where(t => t.Country.ToLower() == filter.Country.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(filter.City))
            {
                tournaments = tournaments.Where(t => t.City.ToLower() == filter.City.ToLower());
            }

            var rankings = new List<Ranking>();
            foreach (var tournament in tournaments)
            {
                foreach (var player in tournament.TournamentUsers)
                {
                    if (rankings.FirstOrDefault(r => r.Player.Id == player.UserId) == null)
                    {
                        rankings.Add(new Ranking { Player = player.User });
                    }

                    var ranking = rankings.FirstOrDefault(r => r.Player.Id == player.UserId);

                    ranking.BigPoints += player.Bp ?? 0;
                    ranking.SmallPoints += player.Sp ?? 0;
                    ranking.TournamentsPlayed++;

                    var scores = PairingManager.CalculatePlayersScores(tournament);
                    if (scores.First().Player.Id == player.Id)
                    {
                        ranking.TournamentsWon++;
                    }
                }
            }

            if (filter.BigPointsMin.HasValue && filter.BigPointsMax.HasValue
                && filter.BigPointsMin.Value >= 0 && filter.BigPointsMax.Value >= 0
                && filter.BigPointsMax >= filter.BigPointsMin)
            {
                rankings = rankings.Where(r =>
                    r.BigPoints >= filter.BigPointsMin.Value
                    && r.BigPoints <= filter.BigPointsMax.Value).ToList();
            }

            if (filter.SmallPointsMin.HasValue && filter.SmallPointsMax.HasValue
                && filter.SmallPointsMin.Value >= 0 && filter.SmallPointsMax.Value >= 0
                && filter.SmallPointsMax >= filter.SmallPointsMin)
            {
                rankings = rankings.Where(r =>
                    r.SmallPoints >= filter.SmallPointsMin.Value
                    && r.SmallPoints <= filter.SmallPointsMax.Value).ToList();
            }

            ViewBag.Filter = filter;

            return View(rankings.Where(r => r.Player.LockoutEnd == null || r.Player.LockoutEnd < DateTimeOffset.Now).ToList());
        }
    }
}