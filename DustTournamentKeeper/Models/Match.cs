﻿using System;
using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public partial class Match
    {
        public int Id { get; set; }
        public int RoundId { get; set; }
        public int PlayerAid { get; set; }
        public int? PlayerBid { get; set; }
        public string Status { get; set; }
        public int BoardTypeId { get; set; }
        public int? Bpa { get; set; }
        public int? Bpb { get; set; }
        public int? Spa { get; set; }
        public int? Spb { get; set; }
        public int? SoSa { get; set; }
        public int? SoSb { get; set; }

        public BoardType BoardType { get; set; }
        public User PlayerA { get; set; }
        public User PlayerB { get; set; }
        public Round Round { get; set; }
    }
}
