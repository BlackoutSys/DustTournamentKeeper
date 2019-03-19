using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using DustTournamentKeeper.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DustTournamentKeeper.Controllers
{
    public class PlayersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ITournamentRepository _repository;

        public PlayersController(ITournamentRepository repository, UserManager<User> userManager)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View(_repository.Users.Include(u => u.Club).ToList());
        }

        public async Task<IActionResult> Details(int? id)
        {
            User user = null;
            if (id == null && User.Identity.IsAuthenticated)
            {
                User userLoggedIn = await _userManager.FindByNameAsync(User.Identity.Name);

                if (userLoggedIn != null)
                {
                    user = GetUser(userLoggedIn.Id);
                }
            }
            else if (id.HasValue)
            {
                user = GetUser(id.Value);
                
            }

            if (user != null)
            {
                return View(new UserViewModel(user));
            }

            return RedirectToAction("Login", "Account");

        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            User user = null;
            if (id == null && User.Identity.IsAuthenticated)
            {
                User userLoggedIn = await _userManager.FindByNameAsync(User.Identity.Name);

                if (userLoggedIn != null)
                {
                    user = GetUser(userLoggedIn.Id);
                }
            }
            else if (id.HasValue)
            {
                user = GetUser(id.Value);
                if (!(user.UserName == User.Identity.Name || User.IsInRole(nameof(RoleEnum.Administrator))))
                {
                    user = null;
                }
            }

            if (user != null)
            {
                return View(new UserViewModel(user));
            }

            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public IActionResult Edit(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    Id = userViewModel.Id,
                    City = userViewModel.City,
                    Country = userViewModel.Country,
                    Name = userViewModel.Name,
                    UserName = userViewModel.UserName,
                    Surname = userViewModel.Surname,
                    Email = userViewModel.Email,
                    ClubId = userViewModel.ClubId
                };

                var oldUser = _repository.Users.FirstOrDefault(u => u.Id == userViewModel.Id);

                if (oldUser == null)
                {
                    return NotFound();
                }

                _repository.Update(oldUser, user);
                return RedirectToAction("Details", new { id = userViewModel.Id });
            }
            else
            {
                userViewModel.ClubsAvailable = _repository.Clubs.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();
                return View(userViewModel);
            }
        }

        private User GetUser(int id)
        {
            return _repository.Users
                    .Include(u => u.Club)
                    .Include(u => u.TournamentUsers).ThenInclude(tu => tu.Tournament).ThenInclude(t => t.TournamentUsers)
                    .Include(u => u.TournamentUsers).ThenInclude(tu => tu.Tournament).ThenInclude(t => t.Game)
                    .Include(u => u.TournamentUsers).ThenInclude(tu => tu.Tournament).ThenInclude(t => t.RoundsNavigation).ThenInclude(r => r.Matches)
                    .Include(u => u.TournamentUsers).ThenInclude(t => t.Block)
                    .Include(u => u.TournamentUsers).ThenInclude(t => t.Faction)
                    .FirstOrDefault(u => u.Id == id);
        }
    }
}