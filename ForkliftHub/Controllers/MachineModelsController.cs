using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ForkliftHub.Data;
using ForkliftHub.Models;

namespace ForkliftHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MachineModelsController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: Models
        public async Task<IActionResult> Index()
        {
            var models = await _context.MachineModels
                .Include(m => m.Brand)
                .ToListAsync();
            return View(models);
        }

        // GET: Models/Create
        public IActionResult Create()
        {
            ViewBag.Brands = new SelectList(_context.Brands, "Id", "Name");
            return View();
        }

        // POST: Models/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MachineModel model)
        {
            if (ModelState.IsValid)
            {
                _context.MachineModels.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Brands = new SelectList(_context.Brands, "Id", "Name", model?.BrandId);
            return View(model);
        }

        // GET: Models/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _context.MachineModels.FindAsync(id);
            if (model == null)
                return NotFound();

            ViewBag.Brands = new SelectList(_context.Brands, "Id", "Name", model.BrandId);
            return View(model);
        }

        // POST: Models/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MachineModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.MachineModels.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelExists(model.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Brands = new SelectList(_context.Brands, "Id", "Name", model?.BrandId);
            return View(model);
        }

        // GET: Models/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _context.MachineModels
                .Include(m => m.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        // POST: Models/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _context.MachineModels.FindAsync(id);
            if (model != null)
            {
                _context.MachineModels.Remove(model);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ModelExists(int id)
        {
            return _context.MachineModels.Any(e => e.Id == id);
        }
    }
}
