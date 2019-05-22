using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DustTournamentKeeper.ViewModels
{
    public class TournamentViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
        public DateTime DateStart { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
        public DateTime DateEnd { get; set; } = DateTime.Now;

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        public string Club { get; set; }
        public int? ClubId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Slogan { get; set; }

        [Required]
        [Range(2, 1000)]
        public int PlayerLimit { get; set; }

        [Required]
        public string Status { get; set; } = nameof(TournamentStatus.Draft);

        [Required]
        [Range(1, 10)]
        public int Rounds { get; set; }

        [Required]
        [Range(1, 100000)]
        public int ArmyPoints { get; set; }

        [DataType(DataType.MultilineText)]
        public string SpecificRules { get; set; }

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        [DataType(DataType.Currency)]
        public double Fee { get; set; }

        [Required]
        public string FeeCurrency { get; set; }

        public string FeeText => Fee.ToString() + " " + FeeCurrency;
        public int? Bpwin { get; set; }
        public int? Bptie { get; set; }
        public int? Bploss { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public int? GameId { get; set; }

        public int? TieBreaker1 { get; set; }
        public int? TieBreaker2 { get; set; }
        public int? TieBreaker3 { get; set; }
        public int? TieBreaker4 { get; set; }


        public string Organizer { get; set; }
        public int OrganizerId { get; set; }
        public int UserId { get; set; }
        public List<RoundViewModel> RoundsList { get; set; } = new List<RoundViewModel>();
        public bool FirstRoundAvailable { get; set; }
        public bool NextRoundAvailable { get; set; }
        public bool FinishAvailable { get; set; }
        public List<PlayerViewModel> PlayersList { get; set; } = new List<PlayerViewModel>();
        public bool Registered { get; set; }
        public bool CanRegister => PlayersList.Count < PlayerLimit;


        public List<BoardType> Boards { get; set; } = new List<BoardType>();
        public List<BoardSelectionFilter> BoardsSelection { get; set; } = new List<BoardSelectionFilter>();
        public List<SelectListItem> ClubsAvailable { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> BoardsAvailable { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> PlayersAvailable { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> GamesAvailable { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> StatusesAvailable { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> TieBreakersAvailable { get; set; } = new List<SelectListItem>();

        public TournamentViewModel()
        {

        }

        public TournamentViewModel(int id, ITournamentRepository repository, int userId)
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
            PrepareViewModel(tournament, userId);
        }

        public TournamentViewModel(Tournament tournament, int userId)
        {
            PrepareViewModel(tournament, userId);
        }

        private void PrepareViewModel(Tournament tournament, int userId)
        {
            Id = tournament.Id;
            DateStart = tournament.DateStart;
            DateEnd = tournament.DateEnd;
            Country = tournament.Country;
            City = tournament.City;
            Address = tournament.Address;
            Club = tournament?.ClubNavigation?.Name ?? tournament?.Club ?? "";
            ClubId = tournament.ClubId;
            Title = tournament.Title;
            Slogan = tournament.Slogan;
            PlayerLimit = tournament.PlayerLimit;
            Status = tournament.Status;
            Rounds = tournament.Rounds;
            ArmyPoints = tournament.ArmyPoints;
            SpecificRules = tournament.SpecificRules;
            Created = tournament.Created;
            LastModified = tournament.LastModified;
            Fee = tournament.Fee;
            FeeCurrency = tournament.FeeCurrency;
            Bpwin = tournament.Bpwin;
            Bptie = tournament.Bptie;
            Bploss = tournament.Bploss;
            Organizer = tournament?.Organizer?.UserName ?? "";
            OrganizerId = tournament.OrganizerId > 0 ? tournament.OrganizerId : userId;
            UserId = userId;
            GameId = tournament.GameId;
            TieBreaker1 = tournament.TieBreaker1;
            TieBreaker2 = tournament.TieBreaker2;
            TieBreaker3 = tournament.TieBreaker3;
            TieBreaker4 = tournament.TieBreaker4;

            Enum.GetValues(typeof(TieBreaker)).Cast<TieBreaker>().ToList().ForEach(e => {
                TieBreakersAvailable.Add(new SelectListItem
                {
                    Text = e.ToString(),
                    Value = ((int)e).ToString()
                });
            });

            FirstRoundAvailable = tournament.OrganizerId == userId && tournament.RoundsNavigation.Count == 0;
            NextRoundAvailable = tournament.OrganizerId == userId && tournament.RoundsNavigation.Count < tournament.Rounds;

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
            PlayersList = TournamentViewModelSorter.SortPlayerScoresUseTieBreakers(PlayersList, tournament);

            Registered = tournament.TournamentUsers.Any(tu => tu.UserId == userId);

            foreach (var tournamentBoardType in tournament.TournamentBoardTypes)
            {
                var boardSelection = BoardsSelection.Find(bs => bs.Id == tournamentBoardType.BoardTypeId);
                if (boardSelection != null)
                {
                    boardSelection.Count++;
                }
            }
        }
    }
}
