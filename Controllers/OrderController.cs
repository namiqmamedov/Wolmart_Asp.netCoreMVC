using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Enums;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels.CartViewModels;
using Wolmart.Ecommerce.ViewModels.OrderViewModels;

namespace Wolmart.Ecommerce.Controllers
{
    [Authorize(Roles = "Member")]
    public class OrderController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context,UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AppUser appUser = await  _userManager.FindByNameAsync(User.Identity.Name);

            List<Cart> carts = await _context.Carts.Include(c=>c.Product).Where(c => c.AppUserID == appUser.Id).ToListAsync();

            Order order = new Order
            {
                Name = appUser.FirstName,
                Surname = appUser.LastName,
                Email = appUser.Email
            };

            OrderVM orderVM = new OrderVM
            {
                Order = order,
                Carts = carts,
            };

            return View(orderVM);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            AppUser appUser = await _userManager.Users.Include(u=>u.Carts).ThenInclude(c=>c.Product).FirstOrDefaultAsync(u=>u.UserName == User.Identity.Name);

            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (Cart cart in appUser.Carts)
            {
                OrderItem orderItem = new OrderItem
                {
                    Price = cart.Product.DiscountedPrice > 0 ? cart.Product.DiscountedPrice : cart.Product.Price,
                    Count = cart.Count,
                    ProductID = cart.ProductID,
                    TotalPrice = cart.Product.DiscountedPrice > 0 ? cart.Product.DiscountedPrice * cart.Count : cart.Product.Price * cart.Count
                };

                orderItems.Add(orderItem);
            }

            order.OrderItems = orderItems;
            order.CreatedAt = DateTime.Now;
            order.AppUserId = appUser.Id;
            order.OrderStatus = OrderStatus.Pending;
            order.TotalPrice = orderItems.Sum(o => o.TotalPrice);

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            //order.UserID

            return RedirectToAction("Index", "home",appUser); 
        }
    }
}
