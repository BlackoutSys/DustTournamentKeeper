using Microsoft.AspNetCore.Identity;

namespace DustTournamentKeeper.Models
{
    public class UserLogin: IdentityUserLogin<int>
    {
        public virtual User User { get; set; }
    }
}
