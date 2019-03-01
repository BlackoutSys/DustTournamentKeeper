using DustTournamentKeeper.Models;

namespace DustTournamentKeeper.ViewModels
{
    public class MatchViewModel
    {
        public string Status { get; set; }
        public string Bpa { get; set; }
        public string Bpb { get; set; }
        public string Spa { get; set; }
        public string Spb { get; set; }
        public string SoSa { get; set; }
        public string SoSb { get; set; }
        public string BoardNumber { get; set; }
        public string BoardName { get; set; }
        public string PlayerA { get; set; }
        public string PlayerB { get; set; }

        public MatchViewModel(Match match)
        {
            Status = match.Status;
            Bpa = match.Bpa.HasValue ? match.Bpa.ToString() : "-";
            Bpb = match.Bpb.HasValue ? match.Bpb.ToString() : "-";
            Spa = match.Spa.HasValue ? match.Spa.ToString() : "-";
            Spb = match.Spa.HasValue ? match.Spb.ToString() : "-";
            SoSa = match.SoSa.HasValue ? match.SoSa.ToString() : "-";
            SoSb = match.SoSb.HasValue ? match.SoSb.ToString() : "-";
            BoardNumber = match.BoardNumber.HasValue ? match.BoardNumber.ToString() : "-";
            BoardName = match.BoardType?.Name ?? "-";
            PlayerA = match.PlayerA.UserName;
            PlayerB = match?.PlayerB?.UserName ?? "Bye";
        }
    }
}
