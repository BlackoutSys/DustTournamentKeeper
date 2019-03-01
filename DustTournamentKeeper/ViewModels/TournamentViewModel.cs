using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public bool FinishAvailable { get; set; }
        public List<PlayerViewModel> PlayersList { get; set; }

        public TournamentViewModel(int id, ITournamentRepository repository, bool userIsOrganizer)
        {
            var tournament = repository.Tournaments
                        .Include(t => t.ClubNavigation)
                        .Include(t => t.Organizer)
                        .Include(t => t.TournamentBoardTypes)
                        .Include(t => t.RoundsNavigation).ThenInclude(r => r.Matches).ThenInclude(m => m.BoardType)
                        .Include(t => t.TournamentUsers).ThenInclude(u => u.User)
                        .Include(t => t.TournamentUsers).ThenInclude(u => u.Block)
                        .Include(t => t.TournamentUsers).ThenInclude(u => u.Faction)
                        .FirstOrDefault(t => t.Id == id);
            PrepareViewModel(tournament, userIsOrganizer);
        }

        public TournamentViewModel(Tournament tournament, bool userIsOrganizer)
        {
            PrepareViewModel(tournament, userIsOrganizer);
        }

        private void PrepareViewModel(Tournament tournament, bool userIsOrganizer)
        {
            Id = tournament.Id;
            DateStart = tournament.DateStart;
            DateEnd = tournament.DateEnd;
            Country = tournament.Country ?? "-";
            City = tournament.City ?? "-";
            Address = tournament.Address ?? "-";
            Club = tournament?.ClubNavigation?.Name ?? tournament?.Club ?? "";
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
            Organizer = tournament?.Organizer?.UserName ?? "-";

            FirstRoundAvailable = userIsOrganizer && tournament.RoundsNavigation.Count == 0;
            NextRoundAvailable = userIsOrganizer && tournament.RoundsNavigation.Count < tournament.Rounds;
            FinishAvailable = userIsOrganizer && tournament.Status == nameof(TournamentStatus.Ongoing);

            RoundsList = new List<RoundViewModel>();
            foreach (var round in tournament.RoundsNavigation)
            {
                RoundsList.Add(new RoundViewModel(round));
            }

            PlayersList = new List<PlayerViewModel>();
            foreach (var player in tournament.TournamentUsers)
            {
                PlayersList.Add(new PlayerViewModel(player));
            }
        }
    }
}
