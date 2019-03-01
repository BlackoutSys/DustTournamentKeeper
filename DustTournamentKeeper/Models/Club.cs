using System;
using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public partial class Club
    {
        public Club()
        {
            Tournaments = new List<Tournament>();
            Users = new List<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Logo { get; set; }
        public string Icon { get; set; }
        public string Slogan { get; set; }
        public string Address { get; set; }

        public virtual IList<Tournament> Tournaments { get; set; }
        public virtual IList<User> Users { get; set; }
    }
}
