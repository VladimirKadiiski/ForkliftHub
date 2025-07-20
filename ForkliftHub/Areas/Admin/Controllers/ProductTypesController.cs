using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Areas.Admin.Controllers
{
    [Area("Admin")]
  
    public class ProductTypesController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            var productTypes = await _context.ProductTypes.ToListAsync();
            return View(productTypes);
        }

        public IActionResult Create() => View(new ProductTypeViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypeViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (_context.ProductTypes.Any(pt => pt.Name == vm.Name))
            {
                ModelState.AddModelError("Name", "A type of product with this name already exists.");
                return View(vm);
            }

            _context.ProductTypes.Add(new ProductType { Name = vm.Name });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var type = await _context.ProductTypes.FindAsync(id);
            if (type == null) return NotFound();

            return View(new ProductTypeViewModel { Id = type.Id, Name = type.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductTypeViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            var type = await _context.ProductTypes.FindAsync(id);
            if (type == null) return NotFound();

            type.Name = vm.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var type = await _context.ProductTypes.FindAsync(id);
            if (type == null) return NotFound();

            return View(new ProductTypeViewModel { Id = type.Id, Name = type.Name });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var type = await _context.ProductTypes.FindAsync(id);
            if (type != null)
            {
                _context.ProductTypes.Remove(type);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
