using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class EnginesController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            var engines = await _context.Engines.ToListAsync();
            return View(engines);
        }

        public IActionResult Create() => View(new EngineViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EngineViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (_context.Engines.Any(e => e.Type == vm.Name))
            {
                ModelState.AddModelError("Name", "An engine with this name already exists.");
                return View(vm);
            }

            _context.Engines.Add(new Engine { Type = vm.Name });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var engine = await _context.Engines.FindAsync(id);
            if (engine == null) return NotFound();

            return View(new EngineViewModel { Id = engine.Id, Name = engine.Type });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EngineViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            var engine = await _context.Engines.FindAsync(id);
            if (engine == null) return NotFound();

            engine.Type = vm.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var engine = await _context.Engines.FindAsync(id);
            if (engine == null) return NotFound();

            return View(new EngineViewModel { Id = engine.Id, Name = engine.Type });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var engine = await _context.Engines.FindAsync(id);
            if (engine != null)
            {
                _context.Engines.Remove(engine);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
