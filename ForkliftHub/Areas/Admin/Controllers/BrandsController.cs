using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ForkliftHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandsController(IBrandService brandService) : Controller
    {
        private readonly IBrandService _brandService = brandService;

        public async Task<IActionResult> Index()
        {
            var brands = await _brandService.GetAllAsync();
            return View(brands);
        }

        public IActionResult Create() => View(new BrandViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (await _brandService.BrandExistsAsync(vm.Name))
            {
                ModelState.AddModelError("Name", "A brand with this name already exists.");
                return View(vm);
            }

            await _brandService.CreateAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();

            var vm = await _brandService.GetByIdAsync(id.Value);
            if (vm is null) return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BrandViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            var updated = await _brandService.UpdateAsync(vm);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var vm = await _brandService.GetByIdAsync(id.Value);
            if (vm is null) return NotFound();

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _brandService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
