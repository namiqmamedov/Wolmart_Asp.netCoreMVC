﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        //private List<SelectListItem> GetCountries()
        //{
        //    var lstCountries = new List<SelectListItem>();

        //    List<Countries> countries = _context.Countries.ToList();

        //    lstCountries = countries.Select(ct => new SelectListItem()
        //    {
        //        Value = ct.ID.ToString(),
        //        Text = ct.Name
        //    }).ToList();

        //    var defItem = new SelectListItem()
        //    {
        //        Value = "",
        //        Text = "----Select Country----"
        //    };

        //    lstCountries.Insert(0, defItem);

        //    return lstCountries;
        //}

        private List<SelectListItem> GetCountries()
        {
            var lstCountries = new List<SelectListItem>();

            List<Countries> countries = _context.Countries.ToList();

            lstCountries = countries.Select(ct => new SelectListItem()
            {
                Value = ct.ID.ToString(),
                Text = ct.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "Please select a country"
            };

            lstCountries.Insert(0, defItem);

            return lstCountries;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //ViewBag.Countries = await _context.Countries.ToListAsync();
            ViewBag.CountryID = GetCountries();
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
            ViewBag.CountryID = GetCountries();
            //ViewBag.Countries = await _context.Countries.ToListAsync();
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

            return RedirectToAction("Index", "home",appUser); 
        }
    }
}
