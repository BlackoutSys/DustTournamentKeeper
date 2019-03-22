using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DustTournamentKeeper.Controllers
{
    [Authorize(Roles = nameof(Roles.Administrator))]
    public class GameController : Controller
    {
        private readonly ITournamentRepository _repository;

        public GameController(ITournamentRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index() => View(_repository.Games.ToList());

        public IActionResult Upsert(int? id)
        {
            var game = id.HasValue ? _repository.Games.FirstOrDefault(g => g.Id == id.Value) : new Game();
            return View(game);
        }

        [HttpPost]
        public IActionResult Upsert(Game game)
        {
            if (!ModelState.IsValid)
            {
                return View(game);
            }

            if (game.Id > 0)
            {
                var oldGame = _repository.Games.FirstOrDefault(g => g.Id == game.Id);
                _repository.Update(oldGame, game);
            }
            else
            {
                _repository.Add(game);
            }

            return RedirectToAction(nameof(GameController.Details), new { id = game.Id });
        }

        public IActionResult Details(int id)
        {
            var game = _repository.Games.FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        public IActionResult Remove(int id)
        {
            var game = _repository.Games.FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            _repository.Delete(game);

            return RedirectToAction(nameof(GameController.Index));
        }
    }
}