namespace DustTournamentKeeper.Models
{
    public class Match
    {
        public int Id { get; set; }
        public int RoundId { get; set; }
        public int PlayerAid { get; set; }
        public int? PlayerBid { get; set; }
        public string Status { get; set; } = "Pending";
        public int BoardTypeId { get; set; }
        public int? Bpa { get; set; }
        public int? Bpb { get; set; }
        public int? Spa { get; set; }
        public int? Spb { get; set; }
        public int? SoSa { get; set; }
        public int? SoSb { get; set; }
        public int? BoardNumber { get; set; }

        public virtual BoardType BoardType { get; set; }
        public virtual User PlayerA { get; set; }
        public virtual User PlayerB { get; set; }
        public virtual Round Round { get; set; }
    }
}
