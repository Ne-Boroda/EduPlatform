using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using BLL_Education.DTO;
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
                if (HttpContext.User.IsInRole("Student") || HttpContext.User.IsInRole("Teacher"))
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    try
                    {
                        var userEnrollments = enrollmentService.GetAllForStudent(userId);

                        var enrolledCourseIds = userEnrollments?.Select(e => e.CourseId).Where(id => id > 0).ToList() ?? new List<int>();

                        var availableCourses = courses.Where(c => !enrolledCourseIds.Contains(c.Id)).ToList();

                        return View(availableCourses);
                    }
                    catch (Exception ex)
                    {
                        return View("Error");
                    }
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
