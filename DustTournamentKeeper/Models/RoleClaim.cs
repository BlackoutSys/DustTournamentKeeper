using Microsoft.AspNetCore.Identity;

namespace DustTournamentKeeper.Models
{
    public class RoleClaim: IdentityRoleClaim<int>
    {
        public virtual Role Role { get; set; }
    }
}
