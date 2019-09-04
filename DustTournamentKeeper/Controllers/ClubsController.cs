using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DustTournamentKeeper.Controllers
{
    public class ClubsController : Controller
    {
        private readonly ITournamentRepository _repository;

        public ClubsController(ITournamentRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View(_repository.Clubs.ToList());
        }

        public IActionResult Details(int id)
        {
            var club = _repository.Clubs.FirstOrDefault(c => c.Id == id);

            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        [Authorize]
        public IActionResult Upsert(int? id)
        {
            var club = id.HasValue ? _repository.Clubs.FirstOrDefault(c => c.Id == id.Value) : new Club();

            return View(club);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Upsert(Club club)
        {
            if (club.Id < 1)
            {
                _repository.Add(club);
            }
            else
            {
                var oldCLub = _repository.Clubs.FirstOrDefault(c => c.Id == club.Id);
                if (oldCLub == null)
                {
                    return NotFound();
                }

                _repository.Update(oldCLub, club);
            }

            return RedirectToAction("Details", new { id = club.Id });
        }

        [Authorize(Roles = nameof(Roles.Administrator))]
        public IActionResult Remove(int id)
        {
            var club = _repository.Clubs.FirstOrDefault(c => c.Id == id);

            if (club == null)
            {
                return NotFound();
            }

            _repository.Delete(club);

            return RedirectToAction(nameof(ClubsController.Index));
        }
    }
}