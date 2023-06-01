using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Extensions;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels;

namespace Wolmart.Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
        public async Task<IActionResult> Create(Category category,IFormFile file)
        {
            ViewBag.MainCategories = await _context.Categories.Where(p => !p.IsDeleted && p.IsMain).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (category.IsMain || category.File == null)
            {
                ModelState.AddModelError("File", "Error - Photo is required");
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

            if (category.File != null)
            {
                if (category.File.ContentType != "image/png")
                {
                    ModelState.AddModelError("File", "Photo type must have been image extension");
                    return View();
                }

                if ((category.File.Length / 1024) > 15)
                {
                    ModelState.AddModelError("File", "Your file must be a maximum of 15 kb");
                    return View();
                }
                
                category.Image = await category.File.CreateAsync(_env,"admin","assets","images");
            }


            category.Name = category.Name.Trim();
            category.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Category category = await _context.Categories.FirstOrDefaultAsync(p=>!p.IsDeleted && p.ID == id);

            if (category == null)
            {
                return NotFound();
            }

            ViewBag.MainCategories = await _context.Categories.Where(p => !p.IsDeleted && p.IsMain).ToListAsync();

            return View(category);
        }

        [HttpPost]
        
        public async Task<IActionResult> Update(int? id, Category category)
        {

            ViewBag.MainCategories = await _context.Categories.Where(p => !p.IsDeleted && p.IsMain).ToListAsync();

            if (id == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid) return View();

            if (id != category.ID) return BadRequest();

            if (category.IsMain && category.Image == null) 
            {
                ModelState.AddModelError("Image", "Photo is required");

                return View(category);
            }
            else
            {
                if(category.ParentID == null || !await _context.Categories.AnyAsync(p => p.ID == category.ParentID && p.IsMain && !p.IsDeleted))
                {
                    ModelState.AddModelError("ParentID", "Main category required");

                    return View(category);
                }
            }

            Category dbCategory = await _context.Categories.FirstOrDefaultAsync(p => !p.IsDeleted && p.ID == category.ID);


            if (dbCategory == null) return NotFound();

            if (await _context.Categories.AnyAsync(p=>p.ID != category.ID && p.Name.ToLower() == category.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "Already exists");

                return View(category);
            }

            dbCategory.Name = category.Name.Trim();
            dbCategory.IsMain = category.IsMain;
            dbCategory.Image = category.Image;
            dbCategory.ParentID = category.IsMain ? null : category.ParentID;
            dbCategory.UpdatedAt = DateTime.UtcNow.AddHours(+4);

            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(int? id, int page)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Category category = await _context.Categories.FirstOrDefaultAsync(p => p.ID == id);

            if (category == null) return NotFound();

            if (category.IsMain)
            {
                List<Category> children = await _context.Categories.Where(p => p.ParentID == category.ID && !p.IsDeleted).ToListAsync();

                foreach (Category item in children)
                {
                    item.IsDeleted = true;
                    item.DeletedAt = DateTime.UtcNow.AddHours(+4);
                }
            }

            category.IsDeleted = true;
            category.DeletedAt = DateTime.UtcNow.AddHours(+4);

            await _context.SaveChangesAsync();

            IQueryable<Category> categories = _context.Categories;

            int itemCount = int.Parse(_context.Settings.FirstOrDefault(x => x.Key == "PageItem").Value);

            return PartialView("_CategoryIndexPartial",PagenationList<Category>.Create(categories, page, itemCount));

        }

        public async Task<IActionResult> Restore(int? id, int page)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Category category = await _context.Categories.FirstOrDefaultAsync(p => p.ID == id && p.IsDeleted);

            if (category == null) return NotFound();

            if (!category.IsMain && await _context.Categories.AnyAsync(p => p.ID == category.ParentID && p.IsDeleted))
            {
                return BadRequest();
            }


            category.IsDeleted = false;
            category.DeletedAt = DateTime.UtcNow.AddHours(+4);

            await _context.SaveChangesAsync();

            IQueryable<Category> categories = _context.Categories;

            int itemCount = int.Parse(_context.Settings.FirstOrDefault(x => x.Key == "PageItem").Value);

            return PartialView("_CategoryIndexPartial", PagenationList<Category>.Create(categories, page, itemCount));

        }
    }
}
