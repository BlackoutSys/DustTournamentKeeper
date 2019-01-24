using System;
using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public partial class BoardTypeToTournament
    {
        public int Id { get; set; }
        public int BoardTypeId { get; set; }
        public int TournamentId { get; set; }
        public int Number { get; set; }

        public BoardType BoardType { get; set; }
        public Tournament Tournament { get; set; }
    }
}
