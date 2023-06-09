using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Models;

namespace Wolmart.Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IDictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);
            
            return View(settings);
        }
        [HttpGet]
        public async Task<IActionResult> Update(string key)
        {
            if (key == null) return BadRequest();

            Setting setting = await _context.Settings.FirstOrDefaultAsync(k => k.Key == key);
            if (setting == null) return NotFound();
            
            return View(setting);
        }
        [HttpPost]
        public async Task<IActionResult> Update(string key,Setting setting) 
        {
            if (!ModelState.IsValid)
            {
                return View(setting);
            }

            if (key == null) return BadRequest();

            if (key != setting.Key) return NotFound();

            Setting dbSetting = await _context.Settings.FirstOrDefaultAsync(k => k.Key == key);

            if (dbSetting == null) return NotFound();   

            dbSetting.Value = setting.Value;

            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }
    }
}
