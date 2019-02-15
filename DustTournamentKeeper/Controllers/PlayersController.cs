using System;
using System.Collections.Generic;
using System.Linq;
using DustTournamentKeeper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DustTournamentKeeper.Controllers
{
    public class PlayersController : Controller
    {
        private readonly ITournamentRepository _repository;

        public PlayersController(ITournamentRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View(_repository.Users.Include(u => u.Club).ToList());
        }
    }
}