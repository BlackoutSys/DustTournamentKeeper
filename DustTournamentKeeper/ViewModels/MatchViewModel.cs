using DustTournamentKeeper.Models;
using System.ComponentModel.DataAnnotations;

namespace DustTournamentKeeper.ViewModels
{
    public class MatchViewModel
    {
        public int Id { get; set; }

        [Required]
        public int RoundId { get; set; }

        public string Status { get; set; }

        public int? Bpa { get; set; }
        public int? Bpb { get; set; }
        public int? Spa { get; set; }
        public int? Spb { get; set; }
        public int? SoSa { get; set; }
        public int? SoSb { get; set; }
        public int? BoardNumber { get; set; }

        [Required]
        public int BoardTypeId { get; set; }

        public string BoardName { get; set; }

        public string PlayerA { get; set; }
        public string PlayerB { get; set; }

        [Required]
        public int PlayerAid { get; set; }

        public int? PlayerBid { get; set; }

        public MatchViewModel()
        {

        }

        public MatchViewModel(Match match)
        {
            Id = match.Id;
            RoundId = match.RoundId;
            Bpa = match.Bpa;
            Bpb = match.Bpb;
            Spa = match.Spa;
            Spb = match.Spb;
            SoSa = match.SoSa;
            SoSb = match.SoSb;
            BoardNumber = match.BoardNumber;
            BoardTypeId = match.BoardTypeId;
            BoardName = match.BoardType?.Name ?? "-";
            PlayerA = match.PlayerA.UserName;
            PlayerB = match?.PlayerB?.UserName ?? "Bye";
            PlayerAid = match.PlayerAid;
            PlayerBid = match.PlayerBid;
        }
    }
}
