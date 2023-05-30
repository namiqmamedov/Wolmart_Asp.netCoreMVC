using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels;

namespace Wolmart.Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        public BrandController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            IQueryable<Brand> query = _context.Brands;

            int itemCount = int.Parse(_context.Settings.FirstOrDefault(x => x.Key == "PageItem").Value);

            //ViewBag.PageCount = (int)Math.Ceiling((decimal)query.Count() / itemCount); 
            //ViewBag.Page = page;
            //List<Brand> brands = await query.Skip((page - 1) * itemCount).Take(itemCount).ToListAsync();
            //ViewBag.ItemCount = itemCount;
            return View(PagenationList<Brand>.Create(query, page, itemCount));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid)  return View();

            if (await _context.Brands.AnyAsync(p => p.Image.ToLower().Trim() == brand.Image.ToLower().Trim() && p.IsDeleted))
            {
                ModelState.AddModelError("Image", $"{brand.Image} is already exists.");
            }

            brand.CreatedAt = DateTime.UtcNow.AddHours(+4);

            await _context.Brands.AddAsync(brand);

            await _context.SaveChangesAsync();

            TempData["success"] = "Great! You have created successfully.";

            return RedirectToAction("index");   
       
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            //if (!ModelState.IsValid) return View();

            if(id == null) return BadRequest();
            
            Brand brand = await _context.Brands.FirstOrDefaultAsync(p=>p.ID == id); 
            
            if (brand == null)  return NotFound();

            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Brand brand)
        {
            if (!ModelState.IsValid) return View(); 

            if(id == null) return BadRequest();

            if (id != brand.ID)
            {
                return BadRequest();
            }

            if (await _context.Brands.AnyAsync(p => p.ID != brand.ID && p.IsDeleted && p.Image.ToLower().Trim() == brand.Image.ToLower().Trim()))
            {
                ModelState.AddModelError("Image", $"{brand.Image} is already exists.");
                return View();
            }

            Brand dbBrand = await _context.Brands.FirstOrDefaultAsync(p => p.ID == id);

            if (dbBrand == null) { return NotFound(); }

            dbBrand.Image = brand.Image;
            dbBrand.UpdatedAt = DateTime.UtcNow.AddHours(+4);

            await _context.SaveChangesAsync();

            return RedirectToAction("index");

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(p => p.ID == id);

            if (brand == null) { return NotFound(); }

            brand.IsDeleted = true;
            brand.DeletedAt = DateTime.UtcNow.AddHours(+4);

            await _context.SaveChangesAsync();

            return PartialView("_BrandIndexPartial", await _context.Brands.Where(p => !p.IsDeleted).ToListAsync());
        }

        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(p => p.ID == id);

            if (brand == null) { return NotFound(); }

            brand.IsDeleted = false;
            brand.DeletedAt = null ;

            await _context.SaveChangesAsync();

            return PartialView("_BrandIndexPartial", await _context.Brands.ToListAsync());
        }
    }
}
