using DustTournamentKeeper.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DustTournamentKeeper.ViewModels
{
    public class BlockViewModel
    {
        public int Id { get; set; }
        public int? GameId { get; set; }
        public string Name { get; set; }
        public string Slogan { get; set; }
        public string Logo { get; set; }
        public string Icon { get; set; }

        public List<SelectListItem> GamesAvailable { get; set; } = new List<SelectListItem>();

        public BlockViewModel()
        {

        }

        public BlockViewModel(Block block)
        {
            Id = block.Id;
            GameId = block.GameId;
            Name = block.Name;
            Slogan = block.Slogan;
            Logo = block.Logo;
            Icon = block.Icon;
        }
    }
}
