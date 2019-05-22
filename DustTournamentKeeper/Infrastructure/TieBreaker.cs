using System.ComponentModel.DataAnnotations;

namespace DustTournamentKeeper.Infrastructure
{
    public enum TieBreaker
    {
        [Display(Name = "Big points")]
        BigPoints = 1,
        [Display(Name = "Small points")]
        SmallPoints,
        [Display(Name = "Strength of schedule")]
        SoS,
        Bye
    }
}
