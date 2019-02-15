using System;
using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public partial class Game
    {
        public Game()
        {
            Block = new HashSet<Block>();
            Faction = new HashSet<Faction>();
            Tournament = new HashSet<Tournament>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Icon { get; set; }
        public string Slogan { get; set; }
        public string Description { get; set; }

        public ICollection<Block> Block { get; set; }
        public ICollection<Faction> Faction { get; set; }
        public ICollection<Tournament> Tournament { get; set; }
    }
}
