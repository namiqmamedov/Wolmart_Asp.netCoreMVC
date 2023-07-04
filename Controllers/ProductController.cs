using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Extensions;
using Wolmart.Ecommerce.Migrations;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels;
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
        public async Task<IActionResult> Index(int? id, string q, string sort, int page, int minPrice, int maxPrice)
        {
            ViewBag.CurrentSortOrder = sort;
            ViewBag.PriceSortParam = String.IsNullOrEmpty(sort) ? "price_desc" : "";
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewData["CurrentFilter"] = q;


            IQueryable<Product> products = _context.Products.Include(p=>p.ProductColorSizes).ThenInclude(p=>p.Color).Where(p => !p.IsDeleted).OrderBy(p => p.ID);

            //Color = await _context.Colors.Where(p=>p.ProductColorSizes.Any(p=>p.ProductID == id)).ToList();

            //switch (sort)
            //{
            //    case "price_desc":
            //        products = products.OrderBy(o => o.Name);    
            //        break;
            //    case "filter_desc":
            //        products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            //        break;
            //    default:
            //        //products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            //        _ = products;
            //        break;
            //}

            return View(PagenationList<Product>.Create(products,page,6));
        }

        //public async Task<IActionResult> PriceFilter(string minPrice, string maxPrice, string sort, int page)
        //{
        //    ViewBag.PriceSortParam = String.IsNullOrEmpty(sort) ? "price_desc" : "";
        //    ViewBag.CurrentSortOrder = sort;
        //    ViewBag.MinPrice = minPrice;
        //    ViewBag.MaxPrice = maxPrice;

        //    IQueryable<Product> products = _context.Products.Where(p => p.Name.ToLower().Contains(minPrice.Trim().ToLower()));

        //    if (!String.IsNullOrEmpty(minPrice))
        //    {
        //        _ = products;
        //    }
        //    if (!String.IsNullOrEmpty(maxPrice))
        //    {
        //        _ = products;
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }

        //    switch (sort)
        //    {
        //        case "price_desc":
        //            products = products.Where(p => p.Name.ToLower().Contains(minPrice.Trim().ToLower())).OrderByDescending(o => o.Name);
        //            break;
        //        default:
        //            _ = products;
        //            break;
        //    }

        //    return View(PagenationList<Product>.Create(products, page, 6));
        //}

        public async Task<IActionResult> Search(string q, string sort, int page)
        {
            ViewBag.PriceSortParam = String.IsNullOrEmpty(sort) ? "price_desc" : "";
            ViewBag.CurrentSortOrder = sort;
            ViewData["CurrentFilter"] = q;

            IQueryable<Product> products = _context.Products.Where(p => p.Name.ToLower().Contains(q.Trim().ToLower()));

            if (!String.IsNullOrEmpty(q))
            {
                _ = products;
            }
            else
            {
                return NotFound();
            }

            switch (sort)
            {
                case "price_desc":
                    products = products.Where(p=>p.Name.ToLower().Contains(q.Trim().ToLower())).OrderByDescending(o => o.Name);
                    break;
                default:
                    _ = products;
                    break;
            }

            return View(PagenationList<Product>.Create(products, page,6));
        }
        public async Task<IActionResult> Detail(int? id,int colorID)
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
                .Include(p => p.ProductColorSizes).ThenInclude(p => p.Color)
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

        public async Task<IActionResult> SearchProduct(string search)
        {
            List<Product> products = await _context.Products
           .Where(p => p.Name.ToLower().Contains(search.Trim().ToLower())).ToListAsync();

            return PartialView("_SearchPartial", products);
        }

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
