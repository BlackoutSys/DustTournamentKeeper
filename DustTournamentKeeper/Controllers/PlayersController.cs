using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using DustTournamentKeeper.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DustTournamentKeeper.Controllers
{
    public class PlayersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ITournamentRepository _repository;
        private readonly RoleManager<Role> _roleManager;

        public PlayersController(ITournamentRepository repository, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _repository = repository;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View(_repository.Users
                .Where(u => u.LockoutEnd == null || u.LockoutEnd < DateTimeOffset.Now)
                .Include(u => u.Club)
                .ToList());
        }

        [Authorize(Roles = nameof(Roles.Administrator))]
        public IActionResult AdminIndex()
        {
            return View(_repository.Users
                .Include(u => u.Club)
                .ToList());
        }

        public async Task<IActionResult> Details(int? id)
        {
            var currentUser = !string.IsNullOrEmpty(User.Identity.Name) ?
                await _userManager.FindByNameAsync(User?.Identity?.Name) :
                null;

            User user = null;
            if (id == null && User.Identity.IsAuthenticated)
            {
                User userLoggedIn = await _userManager.FindByNameAsync(User?.Identity?.Name);

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
                string existingRole = _userManager.GetRolesAsync(user).Result.Single();
                int existingRoleId = _roleManager.Roles.Single(r => r.Name == existingRole).Id;
                return View(new UserViewModel(user)
                {
                    RoleId = existingRoleId,
                    IsAdmin = User?.IsInRole(nameof(Roles.Administrator)) ?? false,
                    IsOwner = currentUser != null ? currentUser.Id == user.Id : false
                });
            }

            return RedirectToAction("Login", "Account");

        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            User user = null;
            if (id == null && User.Identity.IsAuthenticated)
            {
                if (currentUser != null)
                {
                    user = GetUser(currentUser.Id);
                }
            }
            else if (id.HasValue)
            {
                user = GetUser(id.Value);
                if (!(user.UserName == User.Identity.Name || User.IsInRole(nameof(Roles.Administrator))))
                {
                    user = null;
                }
            }

            if (user != null)
            {
                string existingRole = _userManager.GetRolesAsync(user).Result.Single();
                int existingRoleId = _roleManager.Roles.Single(r => r.Name == existingRole).Id;
                var userViewModel = new UserViewModel(user)
                {
                    RoleId = existingRoleId,
                    IsAdmin = User.IsInRole(nameof(Roles.Administrator)),
                    IsOwner = currentUser.Id == user.Id,
                    LockoutEnabled = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.Now
                };

                PrepareViewModel(userViewModel);

                return View(userViewModel);
            }

            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel userViewModel)
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
                    ClubId = userViewModel.ClubId,
                    LockoutEnd = userViewModel.LockoutEnabled.HasValue 
                        && userViewModel.LockoutEnabled.Value 
                        ? (DateTimeOffset?)DateTimeOffset.Now.AddYears(100) 
                        : null
                };

                var oldUser = _repository.Users.FirstOrDefault(u => u.Id == userViewModel.Id);

                if (oldUser == null)
                {
                    return NotFound();
                }

                _repository.Update(oldUser, user);

                if (!string.IsNullOrWhiteSpace(userViewModel.Password))
                {
                    var result = await _userManager.RemovePasswordAsync(oldUser);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddPasswordAsync(oldUser, userViewModel.Password);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(error.Code, error.Description);
                            }
                            PrepareViewModel(userViewModel);
                            return View(userViewModel);
                        }
                    }
                }

                string existingRole = _userManager.GetRolesAsync(user).Result.Single();
                int existingRoleId = _roleManager.Roles.Single(r => r.Name == existingRole).Id;

                if (userViewModel.RoleId > 0 && existingRoleId != userViewModel.RoleId)
                {
                    IdentityResult roleResult = await _userManager.RemoveFromRoleAsync(oldUser, existingRole);
                    if (roleResult.Succeeded)
                    {
                        var applicationRole = _roleManager.Roles.FirstOrDefault(r => r.Id == userViewModel.RoleId);
                        if (applicationRole != null)
                        {
                            IdentityResult newRoleResult = await _userManager.AddToRoleAsync(oldUser, applicationRole.Name);
                            if (newRoleResult.Succeeded)
                            {
                                return RedirectToAction("Details", new { id = userViewModel.Id });
                            }
                        }
                    }
                }

                return RedirectToAction("Details", new { id = userViewModel.Id });
            }
            else
            {
                PrepareViewModel(userViewModel);

                return View(userViewModel);
            }
        }

        [Authorize(Roles=nameof(Roles.Administrator))]
        public IActionResult Remove(int id)
        {
            var user = _repository.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            user.LockoutEnd = DateTimeOffset.Now.AddYears(100);
            _repository.SaveContext();

            return RedirectToAction(nameof(PlayersController.Index));
        }

        private void PrepareViewModel(UserViewModel userViewModel)
        {
            userViewModel.ClubsAvailable = _repository.Clubs.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            userViewModel.RolesAvailable = _repository.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()
            }).ToList();
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