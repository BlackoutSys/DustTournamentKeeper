using DustTournamentKeeper.Models;
using System.Collections.Generic;

namespace DustTournamentKeeper.ViewModels
{
    public class RoundViewModel
    {
        public int Number { get; set; }
        public List<MatchViewModel> Matches { get; set; }

        public RoundViewModel(Round round)
        {
            Number = round.Number;
            Matches = new List<MatchViewModel>();
            foreach (var match in round.Match)
            {
                Matches.Add(new MatchViewModel(match));
            }
        }
    }
}
