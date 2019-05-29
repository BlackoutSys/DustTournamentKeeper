using DustTournamentKeeper.Models;

namespace DustTournamentKeeper.Infrastructure
{
    public class Ranking
    {
        public User Player { get; set; }
        public int BigPoints { get; set; }
        public int SmallPoints { get; set; }
        public int TournamentsPlayed { get; set; }
        public int TournamentsWon { get; set; }
    }
}
