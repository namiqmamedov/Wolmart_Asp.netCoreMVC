using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels;

namespace Wolmart.Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            IQueryable<Category> query = _context.Categories;

            int itemCount = int.Parse(_context.Settings.FirstOrDefault(x => x.Key == "PageItem").Value);

            //ViewBag.PageCount = (int)Math.Ceiling((decimal)query.Count() / itemCount); 
            //ViewBag.Page = page;
            //List<Brand> brands = await query.Skip((page - 1) * itemCount).Take(itemCount).ToListAsync();
            //ViewBag.ItemCount = itemCount;
            return View(PagenationList<Category>.Create(query, page, itemCount));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.MainCategories = await _context.Categories.Where(p=>!p.IsDeleted&& p.IsMain).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            ViewBag.MainCategories = await _context.Categories.Where(p => !p.IsDeleted && p.IsMain).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (category.IsMain || category.Image == null)
            {
                ModelState.AddModelError("Image", "Error - Photo is required");
                return View();
            }

            else
            {
                if(category.ParentID == null || !await _context.Categories.AnyAsync(p=>!p.IsDeleted && p.IsMain && p.ID == category.ParentID))
                {
                    ModelState.AddModelError("ParentID", "Error - Parent is incorrect");
                    return View();
                }
            }

            if(await _context.Categories.AnyAsync(p=>!p.IsDeleted && p.Name == category.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "Error - Name is already exists");
                return View();
            }

            category.Name = category.Name.Trim();
            category.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }
    }
}
