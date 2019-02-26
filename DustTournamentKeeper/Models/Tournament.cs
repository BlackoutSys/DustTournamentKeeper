using System;
using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public class Tournament
    {
        public Tournament()
        {
            BoardTypeToTournament = new List<BoardTypeToTournament>();
            Round = new List<Round>();
            UserToTournament = new List<UserToTournament>();
        }

        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Club { get; set; }
        public int? ClubId { get; set; }
        public string Title { get; set; }
        public string Slogan { get; set; }
        public int PlayerLimit { get; set; }
        public string Status { get; set; }
        public int Rounds { get; set; }
        public int ArmyPoints { get; set; }
        public string SpecificRules { get; set; }
        public int OrganizerId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public double Fee { get; set; }
        public string FeeCurrency { get; set; }
        public int? Bpwin { get; set; }
        public int? Bptie { get; set; }
        public int? Bploss { get; set; }
        public string Country { get; set; }
        public int? GameId { get; set; }

        public Club ClubNavigation { get; set; }
        public Game Game { get; set; }
        public User Organizer { get; set; }
        public IList<BoardTypeToTournament> BoardTypeToTournament { get; set; }
        public IList<Round> Round { get; set; }
        public IList<UserToTournament> UserToTournament { get; set; }
    }
}
