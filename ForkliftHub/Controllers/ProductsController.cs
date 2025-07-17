using ForkliftHub.Data;
using ForkliftHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Controllers
{
    public class ProductsController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: /Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductType)
                .Include(p => p.MachineModel)
                .Include(p => p.Engine)
                .Include(p => p.MastType)
                .ToListAsync();

            return View(products);
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductType)
                .Include(p => p.MachineModel)
                .Include(p => p.Engine)
                .Include(p => p.MastType)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }


        // GET: /Products/Create
        public IActionResult Create()
        {
            ViewData["Brands"] = new SelectList(_context.Brands, "Id", "Name");
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["ProductTypes"] = new SelectList(_context.ProductTypes, "Id", "Name");
            ViewData["Models"] = new SelectList(_context.MachineModels, "Id", "Name");
            ViewData["Engines"] = new SelectList(_context.Engines, "Id", "Type");
            ViewData["MastTypes"] = new SelectList(_context.MastTypes, "Id", "Name");

            return View();
        }


        // POST: /Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Brands"] = new SelectList(_context.Brands, "Id", "Name", product?.Brand);
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", product?.Category);
            ViewData["ProductTypes"] = new SelectList(_context.ProductTypes, "Id", "Name", product?.ProductType);
            ViewData["Models"] = new SelectList(_context.MachineModels, "Id", "Name", product?.MachineModel);
            ViewData["Engines"] = new SelectList(_context.Engines, "Id", "Type", product?.Engine);
            ViewData["MastTypes"] = new SelectList(_context.MastTypes, "Id", "Name", product?.MastType);

            return View(product);
        }

        // GET: Products/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewData["Brands"] = new SelectList(_context.Brands, "Id", "Name", product.Brand);
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", product.Category);
            ViewData["ProductTypes"] = new SelectList(_context.ProductTypes, "Id", "Name", product.ProductType);
            ViewData["Models"] = new SelectList(_context.MachineModels, "Id", "Name", product.MachineModel);
            ViewData["Engines"] = new SelectList(_context.Engines, "Id", "Type", product.Engine);
            ViewData["MastTypes"] = new SelectList(_context.MastTypes, "Id", "Name", product.MastType);


            return View(product);
        }

        // POST: Products/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(p => p.Id == product.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Brands"] = new SelectList(_context.Brands, "Id", "Name", product.Brand);
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", product.Category);
            ViewData["ProductTypes"] = new SelectList(_context.ProductTypes, "Id", "Name", product.ProductType);
            ViewData["Models"] = new SelectList(_context.MachineModels, "Id", "Name", product.MachineModel);
            ViewData["Engines"] = new SelectList(_context.Engines, "Id", "Type", product.Engine);
            ViewData["MastTypes"] = new SelectList(_context.MastTypes, "Id", "Name", product.MastType);

            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductType)
                .Include(p => p.MachineModel)
                .Include(p => p.Engine)
                .Include(p => p.MastType)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }


        // POST: Products/Delete
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