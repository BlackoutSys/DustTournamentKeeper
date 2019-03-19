using DustTournamentKeeper.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DustTournamentKeeper.ViewModels
{
    public class RoundViewModel
    {
        public int Id { get; set; }

        [Required]
        public int TournamentId { get; set; }

        [Required]
        public int Number { get; set; }

        public List<MatchViewModel> Matches { get; set; } = new List<MatchViewModel>();

        public RoundViewModel()
        {

        }

        public RoundViewModel(Round round)
        {
            Id = round.Id;
            TournamentId = round.TournamentId;
            Number = round.Number;

            Matches = new List<MatchViewModel>();
            foreach (var match in round.Matches)
            {
                Matches.Add(new MatchViewModel(match));
            }
        }
    }
}
