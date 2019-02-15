using System;
using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public partial class BoardType
    {
        public BoardType()
        {
            BoardTypeToTournament = new HashSet<BoardTypeToTournament>();
            Match = new HashSet<Match>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<BoardTypeToTournament> BoardTypeToTournament { get; set; }
        public ICollection<Match> Match { get; set; }
    }
}
