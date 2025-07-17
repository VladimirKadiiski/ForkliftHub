using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ForkliftHub.Data;
using ForkliftHub.Models;

namespace ForkliftHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductTypesController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index() => View(await _context.ProductTypes.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productType);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType == null) return NotFound();
            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductType productType)
        {
            if (id != productType.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(productType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productType);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType == null) return NotFound();
            return View(productType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType != null)
            {
                _context.ProductTypes.Remove(productType);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}