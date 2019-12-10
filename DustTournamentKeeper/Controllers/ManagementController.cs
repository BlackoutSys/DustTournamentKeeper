using DustTournamentKeeper.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DustTournamentKeeper.Controllers
{
    [Authorize(Roles = nameof(Roles.Administrator))]
    public class ManagementController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult HowTo() => View();
    }
}