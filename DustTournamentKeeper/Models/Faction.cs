using System;
using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public partial class Faction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Icon { get; set; }
        public string Slogan { get; set; }
        public int? BlockId { get; set; }

        public Block Block { get; set; }
    }
}
