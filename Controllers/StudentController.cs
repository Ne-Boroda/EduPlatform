using AutoMapper;
using BLL_Education.DTO;
using BLL_Education.Profiles;
using BLL_Education.Services;
using EduPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduPlatform.Controllers
{
    public class StudentController : Controller
    {
        private readonly UserManager<UserModel> _userManager;

        private readonly CourseService courseService;
        private readonly EnrollmentService enrollmentService;

        public StudentController(UserManager<UserModel> userManager)
        {
            _userManager = userManager;

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CourseProfile());
                cfg.AddProfile(new EnrollmentProfile());
            }).CreateMapper();

            courseService = new CourseService(mapper);
            enrollmentService = new EnrollmentService(mapper);
        }

        // GET: StudentController
        public async Task<ActionResult> ProfileAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var studentId = user.Id;
            var enrollments = enrollmentService.GetAllForStudent(studentId);
            var courses = enrollments.Select(e => courseService.FindById(e.CourseId)).ToList();

            var model = new StudentProfileModel
            {
                Name = user.UserName,
                Email = user.Email
            };

            return View(model);
        }

        public ActionResult MyCourses()
        {

            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var studentEnrollments = enrollmentService.GetAllForStudent(studentId);

                if (studentEnrollments == null || !studentEnrollments.Any())
                {
                    return View(new List<CourseDTO>());
                }

                var studentCourses = new List<CourseDTO>();
                foreach (var enrollment in studentEnrollments)
                {
                    if (enrollment.CourseId > 0)
                    {
                        var course = courseService.FindById(enrollment.CourseId);
                        if (course != null)
                        {
                            studentCourses.Add(course);
                        }
                    }
                }

                return View(studentCourses);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: StudentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StudentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
