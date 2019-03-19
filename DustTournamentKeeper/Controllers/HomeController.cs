using DustTournamentKeeper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DustTournamentKeeper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITournamentRepository _repository;

        public HomeController(ITournamentRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var games = _repository.Games.ToList();
            return View(games);
        }

        public IActionResult Privacy => View();
    }
}