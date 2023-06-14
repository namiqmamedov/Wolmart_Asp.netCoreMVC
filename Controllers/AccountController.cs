using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Interfaces;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.Repository;
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
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;

        public AccountController(RoleManager<IdentityRole> roleManager, 
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            AppDbContext context,
            IEmailService emailService,
            IConfiguration configuration,
            IAccountRepository accountRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
            _accountRepository = accountRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
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
            else
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

                if (!string.IsNullOrEmpty(token))
                {
                    await _accountRepository.GenerateEmailConfirmationTokenAsync(appUser);
                }
            }

            await _userManager.AddToRoleAsync(appUser, "Member");

            return RedirectToAction("ConfirmEmail", new {email = registerVM.Email});
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(int? id,LoginVM loginVM)
        {

            AppUser appUser = await _userManager.Users.Include(u => u.Carts).FirstOrDefaultAsync(u => u.NormalizedEmail == 
            loginVM.LoginEmail.Trim().ToUpperInvariant() && !u.isAdmin && !u.IsDeleted);
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

                foreach (CartVM cartVM in cartVMs)
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
                            existedCart.Count += cartVM.Count;
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

                HttpContext.Response.Cookies.Append("cart", cart);

                await _context.Carts.AddRangeAsync(carts);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (appUser.Carts != null && appUser.Carts.Count() > 0)
                {
                    List<CartVM> cartVMs = new List<CartVM>(); 
                    foreach (Cart carts in appUser.Carts)
                    {
                        CartVM cartVM = new CartVM
                        {
                            ProductID = carts.ProductID,
                            Count = carts.Count,
                        };

                        cartVMs.Add(cartVM);
                    }

                    cart = JsonConvert.SerializeObject(cartVMs);

                    HttpContext.Response.Cookies.Append("cart", cart);
                }
            }

            if (User.Identity.IsAuthenticated)
            {
                List<CartVM> cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);

                foreach (CartVM cartVM in cartVMs)
                {
                    if (appUser.Carts != null && appUser.Carts.Count() > 0)
                    {
                        Cart existedCart = appUser.Carts.FirstOrDefault(c => c.ProductID == cartVM.ProductID);

                        appUser.Carts.Remove(existedCart);
                    }

                    await _context.SaveChangesAsync();
                }

                cart = JsonConvert.SerializeObject(cartVMs);

                HttpContext.Response.Cookies.Append("cart", cart);
            }

            return RedirectToAction("index", "home");
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
            AppUser appUser = await _userManager.Users.Include(u=>u.Orders).ThenInclude(u=>u.OrderItems).ThenInclude(u=>u.Product).FirstOrDefaultAsync( u=>u.UserName == User.Identity.Name);

            if (appUser == null) return NotFound();

            ProfileVM profileVM = new ProfileVM
            {
                Name = appUser.FirstName,
                Surname = appUser.LastName,
                Email = appUser.Email,
                Username = appUser.UserName
            };

            MemberVM memberVM = new MemberVM
            {
                ProfileVM = profileVM,
                Orders = appUser.Orders,
            };

            return View(memberVM);
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProfileVM profileVM)
        {
            if (!ModelState.IsValid) return View("Profile", profileVM);

            AppUser dbAppUser = await _userManager.FindByNameAsync(User.Identity.Name);

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

            if (profileVM.CurrentPassword != null && profileVM.Password != null)
            {
                if (await _userManager.CheckPasswordAsync(dbAppUser, profileVM.CurrentPassword) && profileVM.CurrentPassword == profileVM.Password)
                {
                    ModelState.AddModelError("", "The old password is the same as the password you typed now!");
                    return View("Profile", profileVM);

                }

                identityResult = await _userManager.ChangePasswordAsync(dbAppUser, profileVM.CurrentPassword, profileVM.Password);

                if (!identityResult.Succeeded)
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    };

                    return View("Profile", profileVM);
                }
            }

            TempData["success"] = "Great! You have updated successfully.";

            return RedirectToAction("Profile");
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token,string email)
        {
            EmailConfirmModel model = new EmailConfirmModel
            {
                Email = email,
            };

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var result =  await _accountRepository.ConfirmEmailAsync(uid, token);

                if (result.Succeeded)
                {
                    model.EmailVerified = true;
                }
            }

            return View(model);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmModel model)
        {
            var user = await _accountRepository.GetUserByEmailAsync(model.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    model.EmailVerified = true;
                    return View(model);
                }

              await _accountRepository.GenerateEmailConfirmationTokenAsync(user);
              model.EmailSent = true;
              ModelState.Clear();

            }
            else
            {
                ModelState.AddModelError("", "Something went wrong");
            }

            return View(model);
        }
    }
}
