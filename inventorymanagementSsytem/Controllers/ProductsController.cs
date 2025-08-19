//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using inventorymanagementSsytem.Data;
//using inventorymanagementSsytem.Models;
//using Microsoft.AspNetCore.Authorization;

//namespace inventorymanagementSsytem.Controllers
//{
//    public class ProductsController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public ProductsController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // GET: Products
//        [Authorize]
//        public async Task<IActionResult> Index()
//        {
//            var products = _context.Products.Include(p => p.Category);
//            return View(await products.ToListAsync());
//        }

//        // GET: Products/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null) return NotFound();

//            var product = await _context.Products
//                .Include(p => p.Category)
//                .FirstOrDefaultAsync(m => m.ProductId == id);

//            if (product == null) return NotFound();

//            return View(product);
//        }

//        // GET: Products/Create
//        public IActionResult Create()
//        {
//            ViewBag.Categories = _context.Categories.ToList();
//            return View();
//        }

//        // POST: Products/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Name,Description,Price,StockLevel,ImageUrl,CategoryId")] Product product)
//        {
//            if (!ModelState.IsValid)
//            {
//                _context.Products.Add(product);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }

//            ViewBag.Categories = _context.Categories.ToList();
//            return View(product);
//        }

//        // GET: Products/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null) return NotFound();

//            var product = await _context.Products.FindAsync(id);
//            if (product == null) return NotFound();

//            ViewBag.Categories = _context.Categories.ToList();
//            return View(product);
//        }

//        // POST: Products/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,Price,StockLevel,ImageUrl,CategoryId")] Product product)
//        {
//            if (id != product.ProductId) return NotFound();

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(product);
//                    await _context.SaveChangesAsync();
//                    return RedirectToAction(nameof(Index));
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!_context.Products.Any(p => p.ProductId == product.ProductId)) return NotFound();
//                    else throw;
//                }
//            }

//            ViewBag.Categories = _context.Categories.ToList();
//            return View(product);
//        }

//        // GET: Products/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null) return NotFound();

//            var product = await _context.Products
//                .Include(p => p.Category)
//                .FirstOrDefaultAsync(m => m.ProductId == id);

//            if (product == null) return NotFound();

//            return View(product);
//        }

//        // POST: Products/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var product = await _context.Products.FindAsync(id);
//            if (product != null)
//            {
//                _context.Products.Remove(product);
//                await _context.SaveChangesAsync();
//            }

//            return RedirectToAction(nameof(Index));
//        }
//    }
//}
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inventorymanagementSsytem.Data;
using inventorymanagementSsytem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace inventorymanagementSsytem.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var products = _context.Products.Include(p => p.Category);
            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                    if (!Directory.Exists(uploadDir))
                        Directory.CreateDirectory(uploadDir);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(uploadDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    product.ImageUrl = "/images/products/" + fileName;
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile ImageFile)
        {
            if (id != product.ProductId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                        if (!Directory.Exists(uploadDir))
                            Directory.CreateDirectory(uploadDir);

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                        var filePath = Path.Combine(uploadDir, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        product.ImageUrl = "/images/products/" + fileName;
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(p => p.ProductId == product.ProductId)) return NotFound();
                    else throw;
                }
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
