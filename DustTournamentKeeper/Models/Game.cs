using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public partial class Game
    {
        public Game()
        {
            Blocks = new List<Block>();
            Factions = new List<Faction>();
            Tournaments = new List<Tournament>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Icon { get; set; }
        public string Slogan { get; set; }
        public string Description { get; set; }

        public virtual IList<Block> Blocks { get; set; }
        public virtual IList<Faction> Factions { get; set; }
        public virtual IList<Tournament> Tournaments { get; set; }
    }
}
