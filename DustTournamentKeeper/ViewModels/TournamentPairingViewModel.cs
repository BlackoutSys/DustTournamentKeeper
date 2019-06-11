using DustTournamentKeeper.Models;
using System.Collections.Generic;

namespace DustTournamentKeeper.ViewModels
{
    public class TournamentPairingViewModel
    {
        public int TournamentId { get; set; }
        public string TournamentTitle { get; set; }
        public int RoundNumber { get; set; }
        public IList<Match> Matches { get; set; }
    }
}
