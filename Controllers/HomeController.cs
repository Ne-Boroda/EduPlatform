using System.Diagnostics;
using AutoMapper;
using BLL_Education.Profiles;
using BLL_Education.Services;
using EduPlatform.Models;
using EduPlatform.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly CourseService courseService;
        private readonly EnrollmentService enrollmentService;

        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public HomeController(UserManager<UserModel> userManager,
                                 SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CourseProfile());
                cfg.AddProfile(new EnrollmentProfile());
            }).CreateMapper();

            courseService = new CourseService(mapper);
            enrollmentService = new EnrollmentService(mapper);
        }

        public ActionResult Index()
        {
            var courses = courseService.GetAll();

            Utilite.SetViewBag(this);
            if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                if (HttpContext.User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                if (HttpContext.User.IsInRole("Student"))
                {
                    return View(courses);
                }
                if (HttpContext.User.IsInRole("Teacher"))
                {
                    return RedirectToAction("Index", "Teacher");
                }
                if (HttpContext.User.IsInRole("Moderator"))
                {
                    return RedirectToAction("Index", "Moderator");
                }
            }
            return View(courses);
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
