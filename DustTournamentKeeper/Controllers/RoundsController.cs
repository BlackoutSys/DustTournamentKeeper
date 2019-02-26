using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DustTournamentKeeper.Models;
using DustTournamentKeeper.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DustTournamentKeeper.Controllers
{
    public class RoundsController : Controller
    {
        private readonly ITournamentRepository _repository;
        private readonly IStringLocalizer<TournamentsController> _localizer;

        public RoundsController(ITournamentRepository repository, IStringLocalizer<TournamentsController> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public ViewResult Edit(int id)
        {
            var round = _repository.Rounds
                .Include(r => r.Match).ThenInclude(m => m.PlayerA)
                .Include(r => r.Match).ThenInclude(m => m.PlayerB)
                .Include(r => r.Match).ThenInclude(m => m.BoardType)
                .FirstOrDefault(r => r.Id == id);

            if (round == null)
            {
                return View("Error");
            }

            return View(round);
        }

        [HttpPost]
        public IActionResult Edit(Round round)
        {
            if (ModelState.IsValid)
            {
                if (round.Id > 0)
                {
                    var oldRound = _repository.Rounds
                        .Include(r => r.Match).ThenInclude(m => m.PlayerA)
                        .Include(r => r.Match).ThenInclude(m => m.PlayerB)
                        .Include(r => r.Match).ThenInclude(m => m.BoardType)
                        .FirstOrDefault(r => r.Id == round.Id);
                    _repository.Update(oldRound, round);
                }
                else
                {
                    return View(round);
                }

                return View("TournamentsView/Details", new TournamentViewModel(round.TournamentId, _repository));
            }
            else
            {
                return View(round);
            }
        }
    }
}