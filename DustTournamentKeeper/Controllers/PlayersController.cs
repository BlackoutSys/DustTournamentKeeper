using DustTournamentKeeper.Models;
using DustTournamentKeeper.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        private User GetUser(int id)
        {
            return _repository.Users
                    .Include(u => u.Club)
                    .Include(u => u.Tournaments).ThenInclude(t => t.Game)
                    .Include(u => u.Tournaments).ThenInclude(t => t.TournamentUsers).ThenInclude(utt => utt.Block)
                    .Include(u => u.Tournaments).ThenInclude(t => t.TournamentUsers).ThenInclude(utt => utt.Faction)
                    .Include(u => u.Tournaments).ThenInclude(t => t.RoundsNavigation).ThenInclude(r => r.Matches)
                    .FirstOrDefault(u => u.Id == id);
        }
    }
}