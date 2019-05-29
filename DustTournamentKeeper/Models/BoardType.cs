﻿using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public partial class BoardType
    {
        public BoardType()
        {
            Matches = new List<Match>();
            TournamentBoardTypes = new List<TournamentBoardType>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual IList<Match> Matches { get; set; }
        public virtual IList<TournamentBoardType> TournamentBoardTypes { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as BoardType).Id == this.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
