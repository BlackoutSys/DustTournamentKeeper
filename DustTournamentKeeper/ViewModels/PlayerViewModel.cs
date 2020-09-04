using DustTournamentKeeper.Models;

namespace DustTournamentKeeper.ViewModels
{
    public class PlayerViewModel
    {
        public int Id { get; set; }
        public int BonusPoints { get; set; }
        public int PenaltyPoints { get; set; }
        public int Bp { get; set; }
        public int Sp { get; set; }
        public int SoS { get; set; }
        public int TotalBigPoints { get; set; }

        public string Block { get; set; }
        public string Faction { get; set; }
        public string User { get; set; }
        public string FullName { get; set; }
        public int UserId { get; set; }

        public PlayerViewModel()
        {

        }

        public PlayerViewModel(TournamentUser player)
        {
            Id = player.Id;
            BonusPoints = player.BonusPoints.HasValue ? player.BonusPoints.Value : 0;
            PenaltyPoints = player.PenaltyPoints.HasValue ? player.PenaltyPoints.Value : 0;
            Bp = player.Bp.HasValue ? player.Bp.Value : 0;
            Sp = player.Sp.HasValue ? player.Sp.Value : 0;
            SoS = player.SoS.HasValue ? player.SoS.Value : 0;
            TotalBigPoints = ((player?.BonusPoints ?? 0)
                - (player?.PenaltyPoints ?? 0)
                + (player?.Bp ?? 0));
            Block = player?.Block?.Name ?? "-";
            Faction = player?.Faction?.Name ?? "-";
            User = player?.User?.UserName ?? player?.User?.Name ?? "-";
            UserId = player?.User?.Id ?? 0;
            FullName = $"{player?.User.Name} {player?.User.Surname}";
        }
    }
}
