using System.Diagnostics;
using EduPlatform.Models;
using EduPlatform.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public HomeController(UserManager<UserModel> userManager,
                                 SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public ActionResult Index()
        {
            Utilite.SetViewBag(this);
            if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                if (HttpContext.User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                if (HttpContext.User.IsInRole("Client"))
                {
                    return RedirectToAction("Index", "Client");
                }
                if (HttpContext.User.IsInRole("Trainer"))
                {
                    return RedirectToAction("Index", "Trainer");
                }
                if (HttpContext.User.IsInRole("Director"))
                {
                    return RedirectToAction("Index", "Director");
                }
                // Guest.
            }
            return View();
        }

        [HttpGet("Home/register")]
        public ActionResult Register()
        {
            Utilite.SetViewBag(this);
            return View();
        }

        [HttpGet("Home/login")]
        public IActionResult Login()
        {
            Utilite.SetViewBag(this);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
