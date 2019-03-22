using DustTournamentKeeper.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DustTournamentKeeper.ViewModels
{
    public class FactionViewModel
    {
        public int Id { get; set; }
        public int? GameId { get; set; }
        public int? BlockId { get; set; }
        public string Name { get; set; }
        public string Slogan { get; set; }
        public string Logo { get; set; }
        public string Icon { get; set; }

        public List<SelectListItem> GamesAvailable { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> BlocksAvailable { get; set; } = new List<SelectListItem>();

        public FactionViewModel()
        {

        }

        public FactionViewModel(Faction faction)
        {
            Id = faction.Id;
            GameId = faction.GameId;
            BlockId = faction.BlockId;
            Name = faction.Name;
            Slogan = faction.Slogan;
            Logo = faction.Logo;
            Icon = faction.Icon;
        }
    }
}
