using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Extensions;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels.CartViewModels;
using Wolmart.Ecommerce.ViewModels.ProductViewModels;

namespace Wolmart.Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }
        public async Task<IActionResult> Page(string q)
        {
            ViewData["CurrentFilter"] = q;

            List<Product> products = await _context.Products.Where(p => p.Name.ToLower().Contains(q.Trim().ToLower())).ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ProductVM productVM = new ProductVM
            {
                Product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.ID == id),

                Feedbacks = await _context.Feedbacks.Where(p=> p.ProductID == id ).ToListAsync(),
                FeedbackImages = await _context.FeedbackImages.Where(p=> p.FeedbackID == p.Feedback.ID  && p.ProductID == id).ToListAsync(),

            };

            if (productVM == null)
            {
                return BadRequest();
            }

            return View(productVM);

        }


        public async Task<IActionResult> DetailForModal(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = await _context.Products
            .Include(p => p.ProductImages)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.ID == id);

            if (product == null)
            {
                return BadRequest();
            }
            return PartialView("_ProductModalPartial", product);
        }

        public async Task<IActionResult> Search(string search)
        {
            List<Product> products = await _context.Products
           .Where(p => p.Name.ToLower().Contains(search.Trim().ToLower())).ToListAsync();

            return PartialView("_SearchPartial", products);
        }

        //public async Task<IActionResult> SearchOtherPage(string searchotherpage)
        //{
        //    List<Product> products = await _context.Products
        // .Where(p => p.Name.ToLower().Contains(searchotherpage.Trim().ToLower())).ToListAsync();

        //    return RedirectToAction("Index","Product");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Feedback(int? id, Feedback feedback)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (appUser == null) return BadRequest();

            if (feedback.Files == null)
            {
                
            }
            else if (feedback.Files != null || feedback.Files.Count() > 0)
            {
                List<FeedbackImage> feedbackImages = new List<FeedbackImage>();

                foreach (IFormFile file in feedback.Files)
                {
                    if (!file.CheckContentType("image/png"))
                    {
                        ModelState.AddModelError("Files", "Image must be png extension");
                    }
                    if (file.CheckFileLength(50))
                    {
                        ModelState.AddModelError("Files", "Image length must be 50kb");
                    }
                    FeedbackImage feedbackImage = new FeedbackImage
                    {
                        Image = await file.CreateAsync(_env, "assets", "images", "feedback")
                    };

                    feedbackImages.Add(feedbackImage);
                }
                feedback.FeedbackImages = feedbackImages;
            }
            feedback.IsDeleted = true;
            feedback.CreatedAt = DateTime.Now;
            feedback.AppUserId = appUser.Id;
            feedback.Email = appUser.Email;
            feedback.Firstname = appUser.FirstName;
            feedback.Lastname = appUser.LastName;
            feedback.Image = appUser?.ProfilePicture;

            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();

            return RedirectToAction("index", "product");
        }
    }
}
