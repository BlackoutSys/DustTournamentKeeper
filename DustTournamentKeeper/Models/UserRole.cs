using Microsoft.AspNetCore.Identity;

namespace DustTournamentKeeper.Models
{
    public class UserRole: IdentityUserRole<int>
    {
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
