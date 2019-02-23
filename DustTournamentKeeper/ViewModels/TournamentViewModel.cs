using DustTournamentKeeper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustTournamentKeeper.ViewModels
{
    public class TournamentViewModel
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Club { get; set; }
        public string Title { get; set; }
        public string Slogan { get; set; }
        public string PlayerLimit { get; set; }
        public string Status { get; set; }
        public string Rounds { get; set; }
        public string ArmyPoints { get; set; }
        public string SpecificRules { get; set; }
        public string Fee { get; set; }
        public string Bpwin { get; set; }
        public string Bptie { get; set; }
        public string Bploss { get; set; }
        public string Country { get; set; }
        public string Organizer { get; set; }
        public List<RoundViewModel> RoundsList { get; set; }
        public bool FirstRoundAvailable { get; set; }
        public bool NextRoundAvailable { get; set; }
        public List<PlayerViewModel> PlayersList { get; set; }

        public TournamentViewModel(Tournament tournament)
        {
            Id = tournament.Id;
            DateStart = tournament.DateStart;
            DateEnd = tournament.DateEnd;
            Country = tournament.Country ?? "-";
            City = tournament.City ?? "-";
            Address = tournament.Address ?? "-";
            Club = tournament?.ClubNavigation.Name ?? tournament.Club ?? "";
            Title = tournament.Title ?? "-";
            Slogan = tournament.Slogan ?? "-";
            PlayerLimit = tournament.PlayerLimit.ToString();
            Status = tournament.Status ?? "-";
            Rounds = tournament.Rounds.ToString();
            ArmyPoints = tournament.ArmyPoints.ToString();
            SpecificRules = tournament.SpecificRules ?? "";
            Fee = tournament.Fee.ToString() + " " + tournament.FeeCurrency ?? ".-";
            Bpwin = tournament?.Bpwin.ToString() ?? "-";
            Bptie = tournament?.Bptie.ToString() ?? "-";
            Bploss = tournament?.Bploss.ToString() ?? "-";
            Organizer = tournament?.Organizer.Nickname ?? "-";

            FirstRoundAvailable = tournament.Round.Count == 0;
            NextRoundAvailable = tournament.Round.Count < tournament.Rounds;

            RoundsList = new List<RoundViewModel>();
            foreach (var round in tournament.Round)
            {
                RoundsList.Add(new RoundViewModel(round));
            }

            PlayersList = new List<PlayerViewModel>();
            foreach (var player in tournament.UserToTournament)
            {
                PlayersList.Add(new PlayerViewModel(player));
            }
        }
    }
}
