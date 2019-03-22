using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using DustTournamentKeeper.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DustTournamentKeeper.Controllers
{
    [Authorize(Roles = nameof(Roles.Administrator))]
    public class BlockController : Controller
    {
        private readonly ITournamentRepository _repository;

        public BlockController(ITournamentRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index() => View(_repository.Blocks.Include(b => b.Game).ToList());

        public IActionResult Upsert(int? id)
        {
            var blockViewModel = id.HasValue
                ? new BlockViewModel(_repository.Blocks.Include(b => b.Game).FirstOrDefault(b => b.Id == id.Value))
                : new BlockViewModel();
            PrepareViewModel(blockViewModel);

            return View(blockViewModel);
        }

        [HttpPost]
        public IActionResult Upsert(BlockViewModel blockViewModel)
        {
            if (!ModelState.IsValid)
            {
                PrepareViewModel(blockViewModel);
                return View(blockViewModel);
            }

            var block = new Block
            {
                Id = blockViewModel.Id > 0 ? blockViewModel.Id : 0,
                GameId = blockViewModel.GameId,
                Name = blockViewModel.Name,
                Slogan = blockViewModel.Slogan,
                Logo = blockViewModel.Logo,
                Icon = blockViewModel.Icon
            };

            if (block.Id > 0)
            {
                var oldBlock = _repository.Blocks.FirstOrDefault(b => b.Id == block.Id);
                _repository.Update(oldBlock, block);
            }
            else
            {
                _repository.Add(block);
            }

            return RedirectToAction(nameof(BlockController.Details), new { id = block.Id });
        }

        private void PrepareViewModel(BlockViewModel blockViewModel)
        {
            blockViewModel.GamesAvailable = _repository.Games.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = g.Id.ToString()
            }).ToList();
        }

        public IActionResult Details(int id)
        {
            var block = _repository.Blocks.Include(b => b.Game).FirstOrDefault(b => b.Id == id);

            if (block == null)
            {
                return NotFound();
            }

            return View(block);
        }

        public IActionResult Remove(int id)
        {
            var block = _repository.Blocks.FirstOrDefault(b => b.Id == id);

            if (block == null)
            {
                return NotFound();
            }

            _repository.Delete(block);

            return RedirectToAction(nameof(BlockController.Index));
        }
    }
}