using System;
using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public partial class User
    {
        public User()
        {
            MatchPlayerA = new HashSet<Match>();
            MatchPlayerB = new HashSet<Match>();
            RoleToUser = new HashSet<RoleToUser>();
            Tournament = new HashSet<Tournament>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }

        public ICollection<Match> MatchPlayerA { get; set; }
        public ICollection<Match> MatchPlayerB { get; set; }
        public ICollection<RoleToUser> RoleToUser { get; set; }
        public ICollection<Tournament> Tournament { get; set; }
    }
}
