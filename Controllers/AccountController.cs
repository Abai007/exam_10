using exam_10.Models;
using exam_10.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam_10.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        private IWebHostEnvironment _appEnvironment;

        private DBContext _db;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, DBContext db, IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
            _appEnvironment = appEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Name,
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {

                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)

        {

            return View(new LoginViewModel { ReturnUrl = returnUrl });

        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = _db.Users.FirstOrDefault(u => u.Email == model.Email);
                var result = await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    false
                    );
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
