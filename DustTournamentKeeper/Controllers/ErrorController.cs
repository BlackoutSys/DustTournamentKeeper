using DustTournamentKeeper.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Net;

namespace DustTournamentKeeper.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly IStringLocalizer<ErrorController> _localizer;

        public ErrorController(IStringLocalizer<ErrorController> localizer)
        {
            _localizer = localizer;
        }

        public IActionResult Error()
        {
            string text = _localizer["GeneralError"];
            switch (Response.StatusCode)
            {
                case (int)HttpStatusCode.NotFound:
                    text = _localizer["PageNotFound"];
                    break;
                case (int)HttpStatusCode.Unauthorized:
                    text = _localizer["Unathorized"];
                    break;
            }
            return View("Error", new ErrorViewModel { ErrorText = text });
        }
    }
}