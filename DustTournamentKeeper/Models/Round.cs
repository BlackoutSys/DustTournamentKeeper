using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public class Round
    {
        public Round()
        {
            Match = new List<Match>();
        }

        public int Id { get; set; }
        public int Number { get; set; }
        public int TournamentId { get; set; }
        public string Comment { get; set; }

        public Tournament Tournament { get; set; }
        public IList<Match> Match { get; set; }
    }
}
