using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Wolmart.Ecommerce.Areas.admin.ViewModels.AccountViewModels;
using Wolmart.Ecommerce.Models;

namespace Wolmart.Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}

            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u=>u.UserName == loginVM.Username.Trim().ToUpperInvariant() && u.isAdmin);

            if (appUser == null)
            {
                ModelState.AddModelError("", "Email or Password is incorrect");
                return View();
            }

           Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, loginVM.RemindMe, true);

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", $"Login to your account has been temporarily disabled. Please wait {(appUser.LockoutEnd.Value-DateTime.UtcNow).TotalMinutes.ToString("F2")} seconds and login again.");
                return View(loginVM);
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or Password is incorrect");
                return View();
            }

            return RedirectToAction("index", "home", new { area = "admin"});
        }
    }
}
