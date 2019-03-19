using DustTournamentKeeper.Infrastructure;
using System;
using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public class Tournament
    {
        public Tournament()
        {
            RoundsNavigation = new List<Round>();
            TournamentBoardTypes = new List<TournamentBoardType>();
            TournamentUsers = new List<TournamentUser>();
        }

        public int Id { get; set; }
        public DateTime DateStart { get; set; } = DateTime.Now;
        public DateTime DateEnd { get; set; } = DateTime.Now;
        public string City { get; set; }
        public string Address { get; set; }
        public string Club { get; set; }
        public int? ClubId { get; set; }
        public string Title { get; set; }
        public string Slogan { get; set; }
        public int PlayerLimit { get; set; }
        public string Status { get; set; } = nameof(TournamentStatus.Draft);
        public int Rounds { get; set; }
        public int ArmyPoints { get; set; }
        public string SpecificRules { get; set; }
        public int OrganizerId { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? LastModified { get; set; }
        public double Fee { get; set; }
        public string FeeCurrency { get; set; }
        public int? Bpwin { get; set; }
        public int? Bptie { get; set; }
        public int? Bploss { get; set; }
        public string Country { get; set; }
        public int? GameId { get; set; }

        public virtual Club ClubNavigation { get; set; }
        public virtual Game Game { get; set; }
        public virtual User Organizer { get; set; }
        public virtual IList<Round> RoundsNavigation { get; set; }
        public virtual IList<TournamentBoardType> TournamentBoardTypes { get; set; }
        public virtual IList<TournamentUser> TournamentUsers { get; set; }
    }
}
