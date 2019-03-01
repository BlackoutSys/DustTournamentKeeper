using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using System.Linq;

namespace DustTournamentKeeper.ViewModels
{
    public class UserHistoryViewModel
    {
        public UserHistoryViewModel(int userId, Tournament tournament)
        {
            Title = tournament.Title ?? "-";
            Game = tournament.Game.Name;
            Country = tournament.Country ?? "-";
            City = tournament.City ?? "-";
            Date = tournament.DateEnd.Date.ToString();

            var userToTournament = tournament.TournamentUsers.FirstOrDefault(u => u.UserId == userId);
            Block = userToTournament?.Block?.Name ?? "-";
            Faction = userToTournament?.Faction?.Name ?? "-";
            TotalBigPoints = (userToTournament?.BonusPoints ?? 0
                - userToTournament?.PenaltyPoints ?? 0
                + userToTournament?.Bp ?? 0).ToString();
            TotalSmallPoints = (userToTournament?.Sp ?? 0).ToString();
            TotalSoSPoints = (userToTournament?.SoS ?? 0).ToString();

            var playersSorted = PairingManager.CalculatePlayersScores(tournament);
            var userScore = playersSorted.FirstOrDefault(p => p.Player.UserId == userId);
            if (userScore == null)
            {
                Standing = "-";
            }
            else
            {
                Standing = (playersSorted.IndexOf(userScore) + 1).ToString();
            }
        }

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
    }
}
