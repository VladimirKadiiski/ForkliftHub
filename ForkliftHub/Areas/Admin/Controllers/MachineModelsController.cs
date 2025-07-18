using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MachineModelsController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: MachineModels
        public async Task<IActionResult> Index()
        {
            var models = await _context.MachineModels
                .Include(m => m.Brand)
                .ToListAsync();
            return View(models);
        }

        // GET: MachineModels/Create
        public IActionResult Create()
        {
            var viewModel = new MachineModelViewModel
            {
                Brands = _context.Brands
                    .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name })
                    .ToList()
            };

            return View(viewModel);
        }

        // POST: MachineModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MachineModelViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Brands = _context.Brands
                    .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name })
                    .ToList();
                return View(vm);
            }

            var model = new MachineModel
            {
                Name = vm.Name,
                BrandId = vm.BrandId
            };

            if (_context.MachineModels.Any(mm => mm.Name == vm.Name))
            {
                ModelState.AddModelError("Name", "A model with this name already exists.");
                return View(vm);
            }

            _context.MachineModels.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: MachineModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var model = await _context.MachineModels.FindAsync(id);
            if (model == null) return NotFound();

            var vm = new MachineModelViewModel
            {
                Id = model.Id,
                Name = model.Name,
                BrandId = model.BrandId,
                Brands = _context.Brands
                    .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name })
                    .ToList()
            };

            return View(vm);
        }

        // POST: MachineModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MachineModelViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                vm.Brands = _context.Brands
                    .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name })
                    .ToList();
                return View(vm);
            }

            var model = await _context.MachineModels.FindAsync(id);
            if (model == null) return NotFound();

            model.Name = vm.Name;
            model.BrandId = vm.BrandId;

            _context.MachineModels.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: MachineModels/Delete/5 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var model = await _context.MachineModels
                .Include(m => m.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
                return NotFound();

            var vm = new MachineModelDeleteViewModel
            {
                Id = model.Id,
                Name = model.Name,
                BrandName = model.Brand.Name
            };

            return View(vm);
        }

        // POST: MachineModels/Delete/5
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
