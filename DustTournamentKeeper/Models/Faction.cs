using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public partial class Faction
    {
        public Faction()
        {
            TournamentUsers = new List<TournamentUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Icon { get; set; }
        public string Slogan { get; set; }
        public int? BlockId { get; set; }
        public int? GameId { get; set; }

        public virtual Block Block { get; set; }
        public virtual Game Game { get; set; }
        public virtual IList<TournamentUser> TournamentUsers { get; set; }
    }
}
