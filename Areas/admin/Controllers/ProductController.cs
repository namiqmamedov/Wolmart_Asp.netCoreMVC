using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Wolmart.Ecommerce.DAL;
using Wolmart.Ecommerce.Extensions;
using Wolmart.Ecommerce.Helpers;
using Wolmart.Ecommerce.Models;
using Wolmart.Ecommerce.ViewModels;

namespace Wolmart.Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]

    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            IQueryable<Product> query = _context.Products;

            int itemCount = int.Parse(_context.Settings.FirstOrDefault(x => x.Key == "PageItem").Value);

            //ViewBag.PageCount = (int)Math.Ceiling((decimal)query.Count() / itemCount); 
            //ViewBag.Page = page;
            //List<Brand> brands = await query.Skip((page - 1) * itemCount).Take(itemCount).ToListAsync();
            //ViewBag.ItemCount = itemCount;
            return View(PagenationList<Product>.Create(query, page, itemCount));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Brands = await _context.Brands.Where(b=>!b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(b=>!b.IsDeleted &&  !b.IsMain).ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();


            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(b => !b.IsDeleted && !b.IsMain).ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();

            if (!ModelState.IsValid) return View();

            if (!await _context.Brands.AnyAsync(b=>!b.IsDeleted && b.ID == product.BrandID))
            {
                ModelState.AddModelError("BrandID","Select a correct brand");

                return View();
            }
            if (product.CategoryID == null)
            {
                ModelState.AddModelError("CategoryID", "You must been select a category");
                return View();
            }

            if (!await _context.Categories.AnyAsync(c => !c.IsDeleted && c.ID == product.CategoryID))
            {
                ModelState.AddModelError("CategoryID ", "Select a correct category");

                return View();
            }

            if (product.ColorIDs.Count() != product.SizeIDs.Count() || product.ColorIDs.Count() != product.Counts.Count() || product.SizeIDs.Count() != product.Counts.Count())
            {
                ModelState.AddModelError("", "Select a correct list");

                return View();
            }

            List<ProductColorSize> productColorSizes = new List<ProductColorSize>();
            
            for (int i = 0; i < product.ColorIDs.Count(); i++)
            {

                if (!await _context.Colors.AnyAsync(c=>c.ID == product.ColorIDs[i]))
                {
                    ModelState.AddModelError("", $"This color id {product.ColorIDs[i]} is incorrect`");

                    return View();
                }

                if (!await _context.Sizes.AnyAsync(c => c.ID == product.SizeIDs[i]))
                {
                    ModelState.AddModelError("", $"This color id {product.SizeIDs[i]} is incorrect`");

                    return View();
                }

                if (product.Counts[i] <= 0)
                {
                    ModelState.AddModelError("", $"Count is incorrect");

                    return View();
                }

                ProductColorSize productColorSize = new ProductColorSize
                {
                    ColorID = product.ColorIDs[i],
                    SizeID = product.SizeIDs[i],
                    Count = product.Counts[i],
                };

                productColorSizes.Add(productColorSize);
            }
            product.ProductColorSizes = productColorSizes;

            if (product.Files != null && product.Files.Count() > 5) 
            {
                ModelState.AddModelError("Files ", "You can select a maximum 5 images");

                return View();
            }

            if (product.MainFile != null)
            {
                if (!product.MainFile.CheckContentType("image/png"))
                {
                    ModelState.AddModelError("MainFile", "Image must be img");
                    return View();
                }
                if (product.MainFile.CheckFileLength(50))
                {
                    ModelState.AddModelError("MainFile", "Image length must be 50kb");
                    return View();
                }

                product.MainImage = await product.MainFile.CreateAsync(_env, "admin", "assets", "images", "product");
            }
            else
            {
                ModelState.AddModelError("MainFile", "Images is required");
                return View();
            }

            if (product.HoverFile != null)
            {
                if (!product.HoverFile.CheckContentType("image/png"))
                {
                    ModelState.AddModelError("HoverFile", "Image must be img");
                    return View();
                }
                if (product.HoverFile.CheckFileLength(50))
                {
                    ModelState.AddModelError("HoverFile", "Image length must be 50kb");
                    return View();
                }

                product.HoverImage = await product.HoverFile.CreateAsync(_env, "admin", "assets", "images", "product");
            }
            else
            {
                ModelState.AddModelError("HoverFile", "Images is required");
                return View();
            }

            if (product.Files != null && product.Files.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();

                foreach (IFormFile file in product.Files)
                {
                    if (!file.CheckContentType("image/png"))
                    {
                        ModelState.AddModelError("Files", "Image must be img");
                        return View();
                    }
                    if (file.CheckFileLength(50))
                    {
                        ModelState.AddModelError("Files", "Image length must be 50kb");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = await file.CreateAsync(_env, "admin", "assets", "images", "product")
                    };

                    productImages.Add(productImage);
                }
                product.ProductImages = productImages; 
            }

            product.Name = product.Name.Trim();
            product.Price = product.Price;
            product.DiscountedPrice = product.DiscountedPrice;
            product.Count = productColorSizes.Sum(x => x.Count);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();


            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.Include(p=>p.ProductImages).FirstOrDefaultAsync(p => p.ID == id);

            if (product == null) return NotFound();

            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(b => !b.IsDeleted && !b.IsMain).ToListAsync();

            return View(product);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int ? id,Product product)
        {
            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(b => !b.IsDeleted && !b.IsMain).ToListAsync();

            //if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            Product dbProduct = await _context.Products.Include(p=>p.ProductImages).FirstOrDefaultAsync(p => p.ID == id);

            if (dbProduct == null) return NotFound();

            if (!await _context.Brands.AnyAsync(b => !b.IsDeleted && b.ID == product.BrandID))
            {
                ModelState.AddModelError("BrandID", "Select a correct brand");

                return View();
            }

            if (product.CategoryID == null)
            {
                ModelState.AddModelError("CategoryID", "You must been select a category");
                return View();
            }

            if (!await _context.Categories.AnyAsync(c => !c.IsDeleted && c.ID == product.CategoryID))
            {
                ModelState.AddModelError("CategoryID ", "Select a correct category");

                return View();
            }

            int canSelectCount = 5 - dbProduct.ProductImages.Count();

            if (product.Files != null && canSelectCount < product.Files.Count())
            {
                ModelState.AddModelError("Files", $"You can  select  a {canSelectCount} items");
                return View();
            }

            if (product.MainFile != null)
            {
                if (!product.MainFile.CheckContentType("image/png"))
                {
                    ModelState.AddModelError("MainFile", "Image must be img");
                    return View();
                }
                if (product.MainFile.CheckFileLength(50))
                {
                    ModelState.AddModelError("MainFile", "Image length must be 50kb");
                    return View();
                }

                FileHelper.DeleteFile(_env, dbProduct.MainImage, "admin", "assets", "images", "product");

                dbProduct.MainImage = await product.MainFile.CreateAsync(_env, "admin", "assets", "images", "product");
            }


            if (product.HoverFile != null)
            {
                if (!product.HoverFile.CheckContentType("image/png"))
                {
                    ModelState.AddModelError("HoverFile", "Image must be img");
                    return View();
                }
                if (product.HoverFile.CheckFileLength(50))
                {
                    ModelState.AddModelError("HoverFile", "Image length must be 50kb");
                    return View();
                }

                FileHelper.DeleteFile(_env, dbProduct.HoverImage, "admin", "assets", "images", "product");

                dbProduct.HoverImage = await product.HoverFile.CreateAsync(_env, "admin", "assets", "images", "product");
            }


            if (product.Files != null && product.Files.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();

                foreach (IFormFile file in product.Files)
                {
                    if (!file.CheckContentType("image/png"))
                    {
                        ModelState.AddModelError("Files", "Image must be img");
                        return View();
                    }
                    if (file.CheckFileLength(50))
                    {
                        ModelState.AddModelError("Files", "Image length must be 50kb");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = await file.CreateAsync(_env, "admin", "assets", "images", "product")
                    };

                    productImages.Add(productImage);
                }
                dbProduct.ProductImages.AddRange(productImages);
            }

            dbProduct.Name = product.Name.Trim();

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ProductImage productImage = await _context.ProductImages.FirstOrDefaultAsync(p => p.ID == id);

            if (productImage == null) return NotFound();

            Product product = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.ID == productImage.ProductID);

            _context.ProductImages.Remove(productImage);

            await _context.SaveChangesAsync();

            FileHelper.DeleteFile(_env, productImage.Image, "assets", "images", "shop");

            return PartialView("_ProductImagePartial", product.ProductImages);
        }
    
        public async Task<IActionResult> ColorSizeItem()
        {
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();


            return PartialView("_ProductColorSizePartial");
        }
    }
}
