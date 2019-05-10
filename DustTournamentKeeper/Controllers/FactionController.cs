using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using DustTournamentKeeper.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustTournamentKeeper.Controllers
{
    [Authorize(Roles = nameof(Roles.Administrator))]
    public class FactionController : Controller
    {
        private readonly ITournamentRepository _repository;

        public FactionController(ITournamentRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index() => View(_repository.Factions
            .Include(f => f.Game)
            .Include(f => f.Block)
            .ToList());

        public IActionResult Upsert(int? id)
        {
            var factionViewModel = id.HasValue
                ? new FactionViewModel(_repository.Factions
                .Include(f => f.Game)
                .Include(f => f.Block)
                .FirstOrDefault(f => f.Id == id.Value))
                : new FactionViewModel();
            PrepareViewModel(factionViewModel);

            return View(factionViewModel);
        }

        [HttpPost]
        public IActionResult Upsert(FactionViewModel factionViewModel)
        {
            if (!ModelState.IsValid)
            {
                PrepareViewModel(factionViewModel);
                return View(factionViewModel);
            }

            var faction = new Faction
            {
                Id = factionViewModel.Id > 0 ? factionViewModel.Id : 0,
                Name = factionViewModel.Name,
                Slogan = factionViewModel.Slogan,
                Logo = factionViewModel.Logo,
                Icon = factionViewModel.Icon,
                GameId = factionViewModel.GameId,
                BlockId = factionViewModel.BlockId
            };

            if (faction.Id > 0)
            {
                var oldFaction = _repository.Factions.FirstOrDefault(b => b.Id == faction.Id);
                _repository.Update(oldFaction, faction);
            }
            else
            {
                _repository.Add(faction);
            }

            return RedirectToAction(nameof(FactionController.Details), new { id = faction.Id });
        }

        private void PrepareViewModel(FactionViewModel factionViewModel)
        {
            factionViewModel.GamesAvailable = _repository.Games.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = g.Id.ToString()
            }).ToList();

            factionViewModel.BlocksAvailable = _repository.Blocks.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = g.Id.ToString()
            }).ToList();
        }

        public IActionResult Details(int id)
        {
            var faction = _repository.Factions
                .Include(f => f.Game)
                .Include(f => f.Block)
                .FirstOrDefault(f => f.Id == id);

            if (faction == null)
            {
                return NotFound();
            }

            return View(faction);
        }

        public IActionResult Remove(int id)
        {
            var faction = _repository.Factions.FirstOrDefault(f => f.Id == id);

            if (faction == null)
            {
                return NotFound();
            }

            _repository.Delete(faction);

            return RedirectToAction(nameof(FactionController.Index));
        }

        public async Task<ActionResult<List<Faction>>> GetAvailableFactions(int blockId)
        {
            var factions = await _repository.Factions.Where(f => f.BlockId == blockId).ToListAsync();

            if (factions == null)
            {
                return NotFound();
            }

            return factions;
        }
    }
}