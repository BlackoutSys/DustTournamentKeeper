using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DustTournamentKeeper.Models;
using Microsoft.AspNetCore.Mvc;

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
    }
}