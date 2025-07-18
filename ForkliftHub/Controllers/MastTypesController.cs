using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MastTypesController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            var types = await _context.MastTypes.ToListAsync();
            return View(types);
        }

        public IActionResult Create() => View(new MastTypeViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MastTypeViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            _context.MastTypes.Add(new MastType { Name = vm.Name });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var mastType = await _context.MastTypes.FindAsync(id);
            if (mastType == null) return NotFound();

            return View(new MastTypeViewModel { Id = mastType.Id, Name = mastType.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MastTypeViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            var mastType = await _context.MastTypes.FindAsync(id);
            if (mastType == null) return NotFound();

            mastType.Name = vm.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var mastType = await _context.MastTypes.FindAsync(id);
            if (mastType == null) return NotFound();

            return View(new MastTypeViewModel { Id = mastType.Id, Name = mastType.Name });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mastType = await _context.MastTypes.FindAsync(id);
            if (mastType != null)
            {
                _context.MastTypes.Remove(mastType);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
