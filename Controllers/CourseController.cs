using AutoMapper;
using BLL_Education.Profiles;
using BLL_Education.Services;
using DAL_Education.Entities;
using EduPlatform.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduPlatform.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseService courseService;
        private readonly CourseCategoriesService courseCategoriesService;
        private readonly CourseLevelsService courseLevelsService;
        private readonly LessonService lessonService;

        public CourseController() 
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CourseProfile());
                cfg.AddProfile(new CourseCategoriesProfile());
                cfg.AddProfile(new CourseLevelsProfile());
                cfg.AddProfile(new LessonProfile());
            }).CreateMapper();

            courseService = new CourseService(mapper);
            courseCategoriesService = new CourseCategoriesService(mapper);
            courseLevelsService = new CourseLevelsService(mapper);
            lessonService = new LessonService(mapper);
        }

        // GET: CourseController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CourseController/Details/5
        public ActionResult Details(int id)
        {
            var course = courseService.FindById(id);

            var courseCategory = courseCategoriesService.FindById(course.CategoryId);
            var courseLevel = courseLevelsService.FindById(course.LevelId);
            var coursesLessons = lessonService.GetAllForCourse(course.Id);

            var cDetails = new CourseDetails { 
                Title = course.Title,
                Description = course.Description,
                Category = courseCategory.Name,
                Level = courseLevel.Name,
                duration = course.duration,
                price = course.price,
                Lessons = coursesLessons,
            };

            return View(cDetails);
        }

        // GET: CourseController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseController/Create
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

        // GET: CourseController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CourseController/Edit/5
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

        // GET: CourseController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CourseController/Delete/5
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
