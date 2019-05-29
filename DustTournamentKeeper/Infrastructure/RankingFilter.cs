using System;

namespace DustTournamentKeeper.Infrastructure
{
    public class RankingFilter
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int? ClubId { get; set; }
        public string ClubName { get; set; }
        public int? BigPointsMin { get; set; }
        public int? BigPointsMax { get; set; }
        public int? SmallPointsMin { get; set; }
        public int? SmallPointsMax { get; set; }
    }
}
