using Microsoft.AspNetCore.Mvc;

namespace DustTournamentKeeper.Controllers
{
    public class ContactController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}