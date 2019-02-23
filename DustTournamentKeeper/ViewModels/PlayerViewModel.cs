using DustTournamentKeeper.Models;

namespace DustTournamentKeeper.ViewModels
{
    public class PlayerViewModel
    {
        public string BonusPoints { get; set; }
        public string PenaltyPoints { get; set; }
        public string Bp { get; set; }
        public string Sp { get; set; }
        public string SoS { get; set; }
        public string TotalBigPoints { get; set; }

        public string Block { get; set; }
        public string Faction { get; set; }
        public string User { get; set; }

        public PlayerViewModel(UserToTournament player)
        {
            BonusPoints = player.BonusPoints.HasValue ? player.BonusPoints.ToString() : "0";
            PenaltyPoints = player.PenaltyPoints.HasValue ? player.PenaltyPoints.ToString() : "0";
            Bp = player.Bp.HasValue ? player.Bp.ToString() : "0";
            Sp = player.Sp.HasValue ? player.Sp.ToString() : "0";
            SoS = player.SoS.HasValue ? player.SoS.ToString() : "0";
            TotalBigPoints = (player?.BonusPoints ?? 0
                - player?.PenaltyPoints ?? 0
                + player?.Bp ?? 0).ToString();
            Block = player?.Block?.Name ?? "-";
            Faction = player?.Faction?.Name ?? "-";
            User = player?.User?.Nickname ?? player?.User?.Name ?? "-";
        }
    }
}
