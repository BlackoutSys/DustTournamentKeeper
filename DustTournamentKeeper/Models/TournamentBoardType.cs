namespace DustTournamentKeeper.Models
{
    public partial class TournamentBoardType
    {
        public int Id { get; set; }
        public int BoardTypeId { get; set; }
        public int TournamentId { get; set; }
        public int Number { get; set; }

        public virtual BoardType BoardType { get; set; }
        public virtual Tournament Tournament { get; set; }
    }
}
