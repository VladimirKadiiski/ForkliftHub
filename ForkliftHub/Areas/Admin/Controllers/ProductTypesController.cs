using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ForkliftHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController(IProductTypeService productTypeService) : Controller
    {
        private readonly IProductTypeService _productTypeService = productTypeService;

        public async Task<IActionResult> Index()
        {
            var types = await _productTypeService.GetAllAsync();
            return View(types);
        }

        public IActionResult Create() => View(new ProductTypeViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypeViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (await _productTypeService.ExistsByNameAsync(vm.Name))
            {
                ModelState.AddModelError("Name", "A type of product with this name already exists.");
                return View(vm);
            }

            await _productTypeService.CreateAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var vm = await _productTypeService.GetByIdAsync(id.Value);
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductTypeViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (!ModelState.IsValid) return View(vm);

            if (await _productTypeService.ExistsByNameAsync(vm.Name, vm.Id))
            {
                ModelState.AddModelError("Name", "A type of product with this name already exists.");
                return View(vm);
            }

            await _productTypeService.UpdateAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vm = await _productTypeService.GetByIdAsync(id.Value);
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productTypeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
