using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            IQueryable<Order> query = _context.Orders.Include(o=>o.OrderItems).ThenInclude(o=>o.Product);

            int itemCount = int.Parse(_context.Settings.FirstOrDefault(x => x.Key == "PageItem").Value);

            //ViewBag.PageCount = (int)Math.Ceiling((decimal)query.Count() / itemCount); 
            //ViewBag.Page = page;
            //List<Brand> brands = await query.Skip((page - 1) * itemCount).Take(itemCount).ToListAsync();
            //ViewBag.ItemCount = itemCount;
            return View(PagenationList<Order>.Create(query, page, itemCount));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Order order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
                .FirstOrDefaultAsync(o => o.ID == id);

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


            return Ok();
        }
        
    }
}
