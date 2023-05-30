﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Models;

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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Brands.Where(p => !p.IsDeleted).ToListAsync());
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

            TempData["success"] = "My name is Inigo Montoya. You killed my father. Prepare to die!";

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

            return RedirectToAction("index");
        }
    }
}
