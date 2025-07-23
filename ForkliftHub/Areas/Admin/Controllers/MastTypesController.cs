using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ForkliftHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MastTypesController(IMastTypeService mastTypeService) : Controller
    {
        private readonly IMastTypeService _mastTypeService = mastTypeService;

        public async Task<IActionResult> Index()
        {
            var types = await _mastTypeService.GetAllMastTypesAsync();
            return View(types);
        }

        public IActionResult Create() => View(new MastTypeViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MastTypeViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (await _mastTypeService.MastTypeExistsAsync(vm.Name))
            {
                ModelState.AddModelError("Name", "A type of mast with this name already exists.");
                return View(vm);
            }

            await _mastTypeService.CreateMastTypeAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var mastType = await _mastTypeService.GetMastTypeByIdAsync(id.Value);
            if (mastType == null) return NotFound();

            return View(mastType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MastTypeViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (!ModelState.IsValid) return View(vm);

            if (await _mastTypeService.MastTypeExistsAsync(vm.Name, vm.Id))
            {
                ModelState.AddModelError("Name", "A type of mast with this name already exists.");
                return View(vm);
            }

            await _mastTypeService.UpdateMastTypeAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var mastType = await _mastTypeService.GetMastTypeByIdAsync(id.Value);
            if (mastType == null) return NotFound();

            return View(mastType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mastTypeService.DeleteMastTypeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
