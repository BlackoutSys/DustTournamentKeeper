using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DustTournamentKeeper.ViewModels
{
    public class RegisterToTournamentViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TournamentId { get; set; }
        public string TournamentTitle { get; set; }
        public int BlockId { get; set; }
        public int? FactionId { get; set; }

        public List<SelectListItem> BlocksAvailable { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> FactionsAvailable { get; set; } = new List<SelectListItem>();
    }
}
