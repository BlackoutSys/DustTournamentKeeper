using System;
using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public partial class Round
    {
        public Round()
        {
            Match = new HashSet<Match>();
        }

        public int Id { get; set; }
        public int Number { get; set; }
        public int TournamentId { get; set; }
        public string Comment { get; set; }

        public Tournament Tournament { get; set; }
        public ICollection<Match> Match { get; set; }
    }
}
