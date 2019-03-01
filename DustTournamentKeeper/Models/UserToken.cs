using Microsoft.AspNetCore.Identity;

namespace DustTournamentKeeper.Models
{
    public class UserToken: IdentityUserToken<int>
    {
        public virtual User User { get; set; }
    }
}
