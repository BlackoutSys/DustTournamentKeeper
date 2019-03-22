using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DustTournamentKeeper.Controllers
{
    [Authorize(Roles = nameof(Roles.Administrator))]
    public class BoardTypeController : Controller
    {
        private readonly ITournamentRepository _repository;

        public BoardTypeController(ITournamentRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index() => View(_repository.BoardTypes.ToList());

        public IActionResult Upsert(int? id)
        {
            var boardType = id.HasValue ? _repository.BoardTypes.FirstOrDefault(b => b.Id == id.Value) : new BoardType();
            return View(boardType);
        }

        [HttpPost]
        public IActionResult Upsert(BoardType boardType)
        {
            if (!ModelState.IsValid)
            {
                return View(boardType);
            }

            if (boardType.Id > 0)
            {
                var oldBoardType = _repository.BoardTypes.FirstOrDefault(b => b.Id == boardType.Id);
                _repository.Update(oldBoardType, boardType);
            }
            else
            {
                _repository.Add(boardType);
            }

            return RedirectToAction(nameof(BoardTypeController.Details), new { id = boardType.Id });
        }

        public IActionResult Details(int id)
        {
            var boardType = _repository.BoardTypes.FirstOrDefault(b => b.Id == id);

            if (boardType == null)
            {
                return NotFound();
            }

            return View(boardType);
        }

        public IActionResult Remove(int id)
        {
            var boardType = _repository.BoardTypes.FirstOrDefault(b => b.Id == id);

            if (boardType == null)
            {
                return NotFound();
            }

            _repository.Delete(boardType);

            return RedirectToAction(nameof(BoardTypeController.Index));
        }
    }
}