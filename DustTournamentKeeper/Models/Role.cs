using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public class Role: IdentityRole<int>
    {
        public Role()
        {
            RoleClaims = new List<RoleClaim>();
            UserRoles = new List<UserRole>();
        }

        public virtual IList<RoleClaim> RoleClaims { get; set; }
        public virtual IList<UserRole> UserRoles { get; set; }
    }
}
