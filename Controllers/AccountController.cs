﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels.AccountViewModels;
using Wolmart.Ecommerce.ViewModels.CartViewModels;

namespace Wolmart.Ecommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;
        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Register()
        {
            return View();    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser appUser = new AppUser
            {
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                UserName = registerVM.Username,
                Email = registerVM.Email,
            };

             IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            await _userManager.AddToRoleAsync(appUser, "Member");

            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginVM loginVM)
        {

            AppUser appUser = await _userManager.Users.Include(u => u.Carts).FirstOrDefaultAsync(u => u.NormalizedEmail == loginVM.LoginEmail.Trim().ToUpperInvariant() && !u.isAdmin && !u.IsDeleted);
            //AppUser appUser = await _userManager.FindByEmailAsync(loginVM.LoginEmail);

            if (appUser == null)
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View(loginVM);
            }

            if (appUser.isAdmin)
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View(loginVM);
            }

            if (!await _userManager.CheckPasswordAsync(appUser, loginVM.LoginPassword))
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View(loginVM);
            }
            await _signInManager.SignInAsync(appUser, loginVM.RemindMe); // RemindMe olaraq veririkki her defe useri yaddasda saxlasin (isPersistent methodu)

            string cart = HttpContext.Request.Cookies["cart"];

            if (!string.IsNullOrWhiteSpace(cart))
            {
                List<CartVM> cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);

                List<Cart> carts = new List<Cart>();

                foreach (CartVM cartVM in cartVMs )
                {
                    if (appUser.Carts != null && appUser.Carts.Count() > 0)
                    {
                        Cart existedCart = appUser.Carts.FirstOrDefault(c => c.ProductID != cartVM.ProductID);

                        if (existedCart == null)
                        {
                            Cart dbCart = new Cart
                            {
                                AppUserID = appUser.Id,
                                ProductID = cartVM.ProductID,
                                Count = cartVM.Count,
                            };

                            carts.Add(dbCart);
                        }
                        else
                        {
                            existedCart.Count = cartVM.Count;
                            cartVM.Count = existedCart.Count;
                        }
                    }
                    else
                    {
                        Cart dbCart = new Cart
                        {
                            AppUserID = appUser.Id,
                            ProductID = cartVM.ProductID,
                            Count = cartVM.Count,
                        };

                        carts.Add(dbCart);
                    }
                }

                cart = JsonConvert.SerializeObject(cartVMs);

                HttpContext.Response.Cookies.Append("cart",cart);

                await _context.Carts.AddRangeAsync(carts);
                await _context.SaveChangesAsync();
            }
            else
            {
                if(appUser.Carts != null && appUser.Carts.Count() > 0)
                {
                    List<CartVM> cartVMs = new List<CartVM>();
                    foreach (Cart carts in appUser.Carts)
                    {
                        CartVM cartVM = new CartVM
                        {
                            ProductID = carts.ProductID,
                            Count = carts.Count
                        };

                        cartVMs.Add(cartVM);
                    }

                    cart = JsonConvert.SerializeObject(cartVMs);

                    HttpContext.Response.Cookies.Append("cart", cart);
                }
            }


            return RedirectToAction("index","home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "home");
        }

        [Authorize(Roles = "Member")]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {

            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

                if (appUser == null) return NotFound();

                ProfileVM profileVM = new ProfileVM
                {
                    Name = appUser.FirstName,
                    Surname = appUser.LastName,
                    Email = appUser.Email,
                    Username = appUser.UserName
                };

                return View(profileVM);
            }

            return RedirectToAction("login");

        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProfileVM profileVM)
        {
            if (!ModelState.IsValid) return View("Profile",profileVM);

            AppUser dbAppUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (dbAppUser.NormalizedUserName != profileVM.Username.Trim().ToUpperInvariant() ||
                dbAppUser.FirstName != profileVM.Name.Trim().ToUpperInvariant() ||
                dbAppUser.LastName != profileVM.Surname.Trim().ToUpperInvariant() ||
                dbAppUser.UserName != profileVM.Username.Trim().ToUpperInvariant() ||
                dbAppUser.Email != profileVM.Email.Trim().ToUpperInvariant())
            {
                dbAppUser.FirstName = profileVM.Name;
                dbAppUser.LastName = profileVM.Surname;
                dbAppUser.UserName = profileVM.Username;
                dbAppUser.Email = profileVM.Email;

                IdentityResult identityResult = await _userManager.UpdateAsync(dbAppUser);

                if (!identityResult.Succeeded)
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    };

                    return View("Profile", profileVM);
                }
                TempData["success"] = "Great! You have updated successfully.";
            }

            if (profileVM.CurrentPassword != null && profileVM.Password != null)
            {
                IdentityResult identityResult = await _userManager.ChangePasswordAsync(dbAppUser, profileVM.CurrentPassword, profileVM.Password);

                if (!identityResult.Succeeded)
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    };

                    return View("Profile", profileVM);
                }
                TempData["successPassword"] = "Great! Your password changed succesfully.";
 
            }

            return RedirectToAction("Profile");
        }

        //public async Task<IActionResult> CreateRole()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });

        //    return Content("success!");
        //}

        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser appUser = new AppUser
        //    {
        //        FirstName = "Namiq",
        //        LastName = "Mamedov",
        //        UserName = "namiq",
        //        Email = "techzip13@gmail.com",
        //        isAdmin = true,
        //    };

        //    await _userManager.CreateAsync(appUser,"namiq1144");

        //    await _userManager.AddToRoleAsync(appUser, "Admin");

        //    return Content("Admin is created");
        //}
    }
}
