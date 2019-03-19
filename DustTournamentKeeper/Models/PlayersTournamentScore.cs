using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public class PlayersTournamentScore
    {
        public TournamentUser Player { get; set; }
        public IList<TournamentUser> Opponents { get; set; } = new List<TournamentUser>();
        public IList<BoardType> Boards { get; set; } = new List<BoardType>();
        public int TotalBigPoints { get; set; }
        public int TotalSmallPoints { get; set; }
        public int TotalSoS { get; set; }
        public int BonusPoints { get; set; }
        public int PenaltyPoints { get; set; }
        public int Byes { get; set; }
        public int Wins { get; set; }
        public int Ties { get; set; }
        public int Loses { get; set; }

        public PlayersTournamentScore(TournamentUser player)
        {
            Player = player;
            TotalBigPoints += player.BonusPoints ?? 0 - player.PenaltyPoints ?? 0;
        }
    }
}
