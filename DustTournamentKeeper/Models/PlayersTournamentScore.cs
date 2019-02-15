using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustTournamentKeeper.Models
{
    public class PlayersTournamentScore
    {
        public UserToTournament Player { get; set; }
        public IList<UserToTournament> Opponents { get; set; } = new List<UserToTournament>();
        public IList<BoardType> Boards { get; set; } = new List<BoardType>();
        public int TotalBigPoints { get; set; }
        public int TotalSmallPoints { get; set; }
        public int TotalSoS { get; set; }
        public bool HadBye { get; set; }

        public PlayersTournamentScore(UserToTournament player)
        {
            Player = player;
            TotalBigPoints += player.BonusPoints ?? 0 - player.PenaltyPoints ?? 0;
        }
    }
}
