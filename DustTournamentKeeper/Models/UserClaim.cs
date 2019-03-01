using Microsoft.AspNetCore.Identity;

namespace DustTournamentKeeper.Models
{
    public class UserClaim: IdentityUserClaim<int>
    {
        public virtual User User { get; set; }
    }
}
