using DustTournamentKeeper.Models;
using DustTournamentKeeper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustTournamentKeeper.Infrastructure
{
    public static class TournamentViewModelSorter
    {
        public static List<PlayerViewModel> SortPlayerScoresUseTieBreakers(List<PlayerViewModel> playerScores, Tournament tournament)
        {
            int tieBreaker1(PlayerViewModel ps)
            {
                int tieBreaker = tournament.TieBreaker1.HasValue ?
                    tournament.TieBreaker1.Value :
                    (int)TieBreaker.BigPoints;

                switch ((TieBreaker)tieBreaker)
                {
                    case TieBreaker.BigPoints:
                        return ps.TotalBigPoints + ps.BonusPoints - ps.PenaltyPoints;
                    case TieBreaker.SmallPoints:
                        return ps.Sp;
                    case TieBreaker.SoS:
                        return ps.SoS;
                    default:
                        return ps.TotalBigPoints + ps.BonusPoints - ps.PenaltyPoints;
                }
            }

            int tieBreaker2(PlayerViewModel ps)
            {
                if (tournament.TieBreaker2 == null) return tieBreaker1(ps);
                switch ((TieBreaker)tournament.TieBreaker2)
                {
                    case TieBreaker.BigPoints:
                        return ps.TotalBigPoints + ps.BonusPoints - ps.PenaltyPoints;
                    case TieBreaker.SmallPoints:
                        return ps.Sp;
                    case TieBreaker.SoS:
                        return ps.SoS;
                    default:
                        return tieBreaker1(ps);
                }
            }

            int tieBreaker3(PlayerViewModel ps)
            {
                if (tournament.TieBreaker3 == null) return tieBreaker2(ps);
                switch ((TieBreaker)tournament.TieBreaker3)
                {
                    case TieBreaker.BigPoints:
                        return ps.TotalBigPoints + ps.BonusPoints - ps.PenaltyPoints;
                    case TieBreaker.SmallPoints:
                        return ps.Sp;
                    case TieBreaker.SoS:
                        return ps.SoS;
                    default:
                        return tieBreaker2(ps);
                }
            }

            int tieBreaker4(PlayerViewModel ps)
            {
                if (tournament.TieBreaker4 == null) return tieBreaker3(ps);
                switch ((TieBreaker)tournament.TieBreaker4)
                {
                    case TieBreaker.BigPoints:
                        return ps.TotalBigPoints + ps.BonusPoints - ps.PenaltyPoints;
                    case TieBreaker.SmallPoints:
                        return ps.Sp;
                    case TieBreaker.SoS:
                        return ps.SoS;
                    default:
                        return tieBreaker3(ps);
                }
            }

            return playerScores.OrderByDescending(tieBreaker1)
                .ThenByDescending(tieBreaker2)
                .ThenByDescending(tieBreaker3)
                .ThenByDescending(tieBreaker4)
                .ToList();
        }
    }
}
