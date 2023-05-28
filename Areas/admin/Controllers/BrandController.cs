using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
            return View(await _context.Brands.ToListAsync());
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

            await _context.Brands.AddAsync(brand);

            await _context.SaveChangesAsync();

            return RedirectToAction("index");   
       
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (!ModelState.IsValid) return View();

            if(id == null) return BadRequest();
            
            Brand brand = await _context.Brands.FirstOrDefaultAsync(p=>p.ID == id); 
            
            if (brand == null) { return NotFound();}

            return View(brand);
        }
    }
}
