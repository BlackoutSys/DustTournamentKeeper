﻿using System;
using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public partial class Club
    {
        public Club()
        {
            Tournament = new HashSet<Tournament>();
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Logo { get; set; }
        public string Icon { get; set; }
        public string Slogan { get; set; }
        public string Address { get; set; }

        public ICollection<Tournament> Tournament { get; set; }
        public ICollection<User> User { get; set; }
    }
}
