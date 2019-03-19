using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using DustTournamentKeeper.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading.Tasks;

namespace DustTournamentKeeper.Controllers
{
    public class AccountController : Controller
    {
        private readonly ITournamentRepository _repository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IStringLocalizer<AccountController> _localizer;

        public AccountController(ITournamentRepository repository, SignInManager<User> signInManager,
            UserManager<User> userManager, RoleManager<Role> roleManager,
            IStringLocalizer<AccountController> localizer)
        {
            _repository = repository;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var registerUserViewModel = new RegisterUserViewModel
            {
                Clubs = _repository.Clubs.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList()
            };

            return View(registerUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel registerUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = registerUserViewModel.Name,
                    UserName = registerUserViewModel.UserName,
                    Surname = registerUserViewModel.Surname,
                    Email = registerUserViewModel.Email,
                    City = registerUserViewModel.City,
                    Country = registerUserViewModel.Country,
                    ClubId = registerUserViewModel.ClubId
                };
                IdentityResult result = await _userManager.CreateAsync(user, registerUserViewModel.Password);
                if (result.Succeeded)
                {
                    var roleId = _repository.Roles.Where(r => r.Name == nameof(Roles.User)).Select(r => r.Id).FirstOrDefault().ToString();
                    var applicationRole = await _roleManager.FindByIdAsync(roleId);
                    if (applicationRole != null)
                    {
                        IdentityResult roleResult = await _userManager.AddToRoleAsync(user, applicationRole.NormalizedName);
                        if (roleResult.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return View(registerUserViewModel);
                }

                return RedirectToLocal(ViewData["ReturnUrl"]?.ToString());
            }
            else
            {
                registerUserViewModel.Clubs = _repository.Clubs.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();
                return View(registerUserViewModel);
            }
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, _localizer["InvalidLogin"]);
                    return View(model);
                }
            }
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult GenerateClubs()
        {
            TestDataGenerator.GenerateClubs(5, _repository);
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> GenerateUsers()
        {
            await TestDataGenerator.GenerateUsers(10, _repository, _userManager, _roleManager);
            return RedirectToAction("Login");
        }

        public IActionResult GenerateBoards()
        {
            TestDataGenerator.GenerateBoards(5, _repository);
            return RedirectToAction("Login");
        }

        public IActionResult GenerateTournaments()
        {
            TestDataGenerator.GenerateTournaments(5, _repository);
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> GenerateBundle()
        {
            TestDataGenerator.GenerateClubs(5, _repository);
            await TestDataGenerator.GenerateUsers(30, _repository, _userManager, _roleManager);
            await TestDataGenerator.GenerateAdmin(_repository, _userManager, _roleManager);
            TestDataGenerator.GenerateBoards(10, _repository);
            TestDataGenerator.GenerateTournaments(5, _repository);
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> NukeDb()
        {
            await TestDataGenerator.NukeDb(_repository, _userManager);
            return RedirectToAction("Login");
        }
    }
}