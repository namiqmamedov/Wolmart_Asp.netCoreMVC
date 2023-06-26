using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Enums;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels;

namespace Wolmart.Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1)
        {

            //List<Order> orders = _context.Orders
            //    .Include(c => c.Countries)
            //    .ToList();
            //ViewBag.Countries = await _context.Countries.ToListAsync();

            IQueryable<Order> query = _context.Orders.Include(o=>o.OrderItems).ThenInclude(o=>o.Product);

            int itemCount = int.Parse(_context.Settings.FirstOrDefault(x => x.Key == "PageItem").Value);

            //ViewBag.PageCount = (int)Math.Ceiling((decimal)query.Count() / itemCount); 
            //ViewBag.Page = page;
            //List<Brand> brands = await query.Skip((page - 1) * itemCount).Take(itemCount).ToListAsync();
            //ViewBag.ItemCount = itemCount;
            return View(PagenationList<Order>.Create(query, page, itemCount));
        }
        private string GetCountryName(int CountryID)
        {
            string countryName = _context.Countries.Where(ct => ct.ID == CountryID).SingleOrDefault().Name;
            return countryName;
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            //Order orders = GetCustomer((int)id);

            //ViewBag.Countries = await _context.Countries.ToListAsync();

            //List<Order> orders = _context.Orders
            //    .Include(c => c.Countries)
            //    .ToList();
            if (id == null) return BadRequest();

            Order order = await _context.Orders
                .Include(o => o.Countries)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
                .FirstOrDefaultAsync(o => o.ID == id);

            ViewBag.CountryName = GetCountryName(order.CountryID);

            if (order == null) return NotFound();

            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,OrderStatus orderStatus,string Comment)
        {
            if (id == null) return BadRequest();

            Order order = await _context.Orders
            .FirstOrDefaultAsync(o => o.ID == id);

            if (order == null) return NotFound();

            order.OrderStatus = orderStatus;
            order.Comment = Comment;

            await _context.SaveChangesAsync();

            return RedirectToAction("index") ;
        }
        
    }
}
