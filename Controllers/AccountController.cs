using EduPlatform.Contexts;
using EduPlatform.Models;
using EduPlatform.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public AccountController(UserManager<UserModel> userManager,
                                 SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("Account/register")]
        public ActionResult Register()
        {
            Utilite.SetViewBag(this);
            return View();
        }

        [HttpGet("Account/login")]
        public IActionResult Login()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = new UserModel { login = model.Email, UserName = model.UserName, Email = model.Email, password = model.Password };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded && (await _userManager.AddToRoleAsync(user, "Student")).Succeeded)
                {
                    return RedirectToAction("Index", "Student");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            Utilite.SetViewBag(this);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(
                        user.UserName,
                        model.Password,
                        false,
                        false);

                    if (result.Succeeded)
                    {
                        if (model.isTeacher && await _userManager.IsInRoleAsync(user, "Teacher"))
                        {
                            TempData["IsLoggedIn"] = true;
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["IsLoggedIn"] = true;
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неверный логин или пароль!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не найден!");
                }
            }
            Utilite.SetViewBag(this);
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            Utilite.SetViewBag(this);
            return RedirectToAction("Index", "Home");
        }
    }
}   
