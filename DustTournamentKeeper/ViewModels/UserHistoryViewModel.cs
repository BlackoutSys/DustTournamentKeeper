using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using System.Collections.Generic;
using System.Linq;

namespace DustTournamentKeeper.ViewModels
{
    public class UserHistoryViewModel
    {
        public int TournamentId { get; set; }
        public string Title { get; set; }
        public string Game { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Date { get; set; }
        public string Block { get; set; }
        public string Faction { get; set; }
        public string Standing { get; set; }
        public string TotalBigPoints { get; set; }
        public string TotalSmallPoints { get; set; }
        public string TotalSoSPoints { get; set; }

        public UserHistoryViewModel(int userId, Tournament tournament)
        {
            TournamentId = tournament.Id;
            Title = tournament.Title ?? "-";
            Game = tournament.Game.Name;
            Country = tournament.Country ?? "-";
            City = tournament.City ?? "-";
            Date = tournament.DateEnd.Date.ToString();

            var userToTournament = tournament.TournamentUsers.FirstOrDefault(u => u.UserId == userId);
            Block = userToTournament?.Block?.Name ?? "-";
            Faction = userToTournament?.Faction?.Name ?? "-";
            TotalBigPoints = ((userToTournament?.BonusPoints ?? 0)
                - (userToTournament?.PenaltyPoints ?? 0)
                + (userToTournament?.Bp ?? 0)).ToString();
            TotalSmallPoints = (userToTournament?.Sp ?? 0).ToString();
            TotalSoSPoints = (userToTournament?.SoS ?? 0).ToString();


            var playersSorted = new List<PlayerViewModel>();
            foreach (var player in tournament.TournamentUsers)
            {
                playersSorted.Add(new PlayerViewModel(player));
            }
            playersSorted = playersSorted.OrderByDescending(pl => pl.TotalBigPoints)
                .ThenByDescending(pl => pl.Bp)
                .ThenByDescending(pl => pl.Sp)
                .ThenByDescending(pl => pl.SoS).ToList();

            var userScore = playersSorted.FirstOrDefault(p => p.UserId == userId);
            if (userScore == null)
            {
                Standing = "-";
            }
            else
            {
                Standing = (playersSorted.IndexOf(userScore) + 1).ToString();
            }
        }

    }
}
