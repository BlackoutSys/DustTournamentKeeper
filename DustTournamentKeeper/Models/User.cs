using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public class User : IdentityUser<int>
    {
        public User()
        {
            MatchesPlayerA = new List<Match>();
            MatchesPlayerB = new List<Match>();
            TournamentUsers = new List<TournamentUser>();
            Tournaments = new List<Tournament>();
            UserClaims = new List<UserClaim>();
            UserLogins = new List<UserLogin>();
            UserRoles = new List<UserRole>();
            UserTokens = new List<UserToken>();
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int? ClubId { get; set; }

        public virtual Club Club { get; set; }
        public virtual IList<Match> MatchesPlayerA { get; set; }
        public virtual IList<Match> MatchesPlayerB { get; set; }
        public virtual IList<TournamentUser> TournamentUsers { get; set; }
        public virtual IList<Tournament> Tournaments { get; set; }
        public virtual IList<UserClaim> UserClaims { get; set; }
        public virtual IList<UserLogin> UserLogins { get; set; }
        public virtual IList<UserRole> UserRoles { get; set; }
        public virtual IList<UserToken> UserTokens { get; set; }
    }
}
