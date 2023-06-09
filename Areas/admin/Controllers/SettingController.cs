using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Models;

namespace Wolmart.Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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

        [HttpPost]
        public ActionResult UploadImage(List<IFormFile> upload)
        {
            var filepath = "";

            foreach (IFormFile photo in Request.Form.Files)
            {
                string serverMapPath = Path.Combine(_env.WebRootPath, "assets", "images", photo.FileName);
                using (var stream = new FileStream(serverMapPath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }
                filepath = "/assets/" + "images/" + photo.FileName;
            }

            return Json(new { url = filepath });
        }
    }
}
