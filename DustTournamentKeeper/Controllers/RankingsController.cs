﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DustTournamentKeeper.Models;
using Microsoft.AspNetCore.Mvc;

namespace DustTournamentKeeper.Controllers
{
    public class RankingsController : Controller
    {
        private ITournamentRepository _repository;

        public RankingsController(ITournamentRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}