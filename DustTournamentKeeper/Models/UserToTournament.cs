using System;
using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public partial class UserToTournament
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TournamentId { get; set; }
        public int? BonusPoints { get; set; }
        public int? PenaltyPoints { get; set; }
        public int? Bp { get; set; }
        public int? Sp { get; set; }
        public int? SoS { get; set; }
        public int? BlockId { get; set; }
        public int? FactionId { get; set; }

        public Block Block { get; set; }
        public Faction Faction { get; set; }
        public Tournament Tournament { get; set; }
        public User User { get; set; }
    }
}
