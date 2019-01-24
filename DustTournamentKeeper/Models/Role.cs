using System;
using System.Collections.Generic;

namespace DustTournamentKeeper.Models
{
    public partial class Role
    {
        public Role()
        {
            RoleToUser = new HashSet<RoleToUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<RoleToUser> RoleToUser { get; set; }
    }
}
