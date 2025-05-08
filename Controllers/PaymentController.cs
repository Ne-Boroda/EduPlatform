using AutoMapper;
using BLL_Education.DTO;
using BLL_Education.Interfaces;
using BLL_Education.Profiles;
using BLL_Education.Services;
using DAL_Education.Entities;
using EduPlatform.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduPlatform.Controllers
{
    public class PaymentController : Controller
    {
        private readonly CourseService courseService;
        private readonly PaymentsService paymentsService;
        private readonly EnrollmentService enrollmentService;

        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public PaymentController(UserManager<UserModel> userManager,
                                 SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CourseProfile());
                cfg.AddProfile(new PaymentsProfile());
                cfg.AddProfile(new EnrollmentProfile());
            }).CreateMapper();

            courseService = new CourseService(mapper);
            paymentsService = new PaymentsService(mapper);
            enrollmentService = new EnrollmentService(mapper);
        }

        // GET: PaymentController
        public ActionResult PayPage(int id)
        {
            return View(id);
        }

        // GET: PaymentController/Enroll/5
        public ActionResult Enroll(int id)
        {
            if (HttpContext.User.IsInRole("Student") || HttpContext.User.IsInRole("Teacher"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                try
                {
                    var newEnrollment = new EnrollmentDTO
                    {
                        UserId = userId,
                        CourseId = id,
                        EnrollmentDate = DateTime.Now,
                        StatusId = 4
                    };

                    var createdEnrollment = enrollmentService.AddReturn(newEnrollment);
                    int enrollmentId = createdEnrollment.Id;

                    var course = courseService.FindById(id);
                    double amount = course.price;

                    paymentsService.ProcessPayment(enrollmentId, amount);

                    return RedirectToAction("MyCourses", "Student");
                }
                catch (Exception ex)
                {
                    return View("Error");
                }
            }
            return View("Error");
        }

        // GET: PaymentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentController/Create
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

        // GET: PaymentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PaymentController/Edit/5
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

        // GET: PaymentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PaymentController/Delete/5
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
