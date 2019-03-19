using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public class Round
    {
        public Round()
        {
            Matches = new List<Match>();
        }

        public int Id { get; set; }
        public int Number { get; set; }
        public int TournamentId { get; set; }

        public virtual Tournament Tournament { get; set; }
        public virtual IList<Match> Matches { get; set; }
    }
}
