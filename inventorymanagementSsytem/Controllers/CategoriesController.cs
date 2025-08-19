//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using inventorymanagementSsytem.Data;
//using inventorymanagementSsytem.Models;

//namespace inventorymanagementSsytem.Controllers
//{
//    public class CategoriesController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public CategoriesController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // GET: Categories
//        public async Task<IActionResult> Index()
//        {
//            return View(await _context.Categories.ToListAsync());
//        }

//        // GET: Categories/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null) return NotFound();

//            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
//            if (category == null) return NotFound();

//            return View(category);
//        }

//        // GET: Categories/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: Categories/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Name")] Category category)
//        {
//            if (!ModelState.IsValid)
//            {
//                _context.Add(category);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(category);
//        }

//        // GET: Categories/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null) return NotFound();

//            var category = await _context.Categories.FindAsync(id);
//            if (category == null) return NotFound();

//            return View(category);
//        }

//        // POST: Categories/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name")] Category category)
//        {
//            if (id != category.CategoryId) return NotFound();

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(category);
//                    await _context.SaveChangesAsync();
//                    return RedirectToAction(nameof(Index));
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!CategoryExists(category.CategoryId)) return NotFound();
//                    else throw;
//                }
//            }
//            return View(category);
//        }

//        // GET: Categories/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null) return NotFound();

//            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
//            if (category == null) return NotFound();

//            return View(category);
//        }

//        // POST: Categories/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var category = await _context.Categories.FindAsync(id);
//            if (category != null)
//            {
//                _context.Categories.Remove(category);
//                await _context.SaveChangesAsync();
//            }

//            return RedirectToAction(nameof(Index));
//        }

//        private bool CategoryExists(int id)
//        {
//            return _context.Categories.Any(c => c.CategoryId == id);
//        }
//    }
//}
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using inventorymanagementSsytem.Data;
using inventorymanagementSsytem.Models;

namespace inventorymanagementSsytem.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoriesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null) return NotFound();

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images/categories");
                    Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    category.ImageUrl = "/images/categories/" + fileName;
                }

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category, IFormFile ImageFile)
        {
            if (id != category.CategoryId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile != null)
                    {
                        string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images/categories");
                        Directory.CreateDirectory(folder);

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                        string filePath = Path.Combine(folder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        category.ImageUrl = "/images/categories/" + fileName;
                    }

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.CategoryId == id);
        }
    }
}
