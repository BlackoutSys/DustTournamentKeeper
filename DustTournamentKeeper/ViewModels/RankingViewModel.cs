using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using System.Collections.Generic;

namespace DustTournamentKeeper.ViewModels
{
    public class RankingViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TournamentsNumber { get; set; }
        public int TotalBigPoints { get; set; }
        public int Wins { get; set; }
        public int Ties { get; set; }
        public int Loses { get; set; }
        public int Byes { get; set; }

        public RankingViewModel(IList<Tournament> tournaments, int userId, string userName)
        {
            UserId = userId;
            UserName = userName;
            CalculateRanking(tournaments, userId);
        }

        private void CalculateRanking(IList<Tournament> tournaments, int userId)
        {
            foreach (var tournament in tournaments)
            {
                var playersSorted = PairingManager.CalculatePlayersScores(tournament);
                var score = playersSorted.Find(p => p.Player.Id == userId);

                TournamentsNumber++;
                TotalBigPoints += score.TotalBigPoints;
                Wins += score.Wins;
                Ties += score.Ties;
                Loses += score.Loses;
                Byes += score.Byes;
            }
        }
    }
}
