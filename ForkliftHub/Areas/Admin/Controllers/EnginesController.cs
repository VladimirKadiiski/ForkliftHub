using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ForkliftHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EnginesController(IEngineService engineService) : Controller
    {
        private readonly IEngineService _engineService = engineService;

        public async Task<IActionResult> Index()
        {
            var engines = await _engineService.GetAllEnginesAsync();
            return View(engines);
        }

        public IActionResult Create() => View(new EngineViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EngineViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (await _engineService.EngineExistsAsync(vm.Name))
            {
                ModelState.AddModelError("Name", "An engine with this name already exists.");
                return View(vm);
            }

            await _engineService.CreateEngineAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var vm = await _engineService.GetEngineByIdAsync(id.Value);
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EngineViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            if (await _engineService.EngineExistsAsync(vm.Name, id))
            {
                ModelState.AddModelError("Name", "An engine with this name already exists.");
                return View(vm);
            }

            await _engineService.UpdateEngineAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vm = await _engineService.GetEngineByIdAsync(id.Value);
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _engineService.DeleteEngineAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
