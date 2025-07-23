using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ForkliftHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController(ICategoryService categoryService) : Controller
    {
        private readonly ICategoryService _categoryService = categoryService;

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        public IActionResult Create() => View(new CategoryViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (await _categoryService.CategoryExistsAsync(vm.Name))
            {
                ModelState.AddModelError("Name", "A category with this name already exists.");
                return View(vm);
            }

            await _categoryService.CreateCategoryAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var vm = await _categoryService.GetCategoryByIdAsync(id.Value);
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            if (await _categoryService.CategoryExistsAsync(vm.Name, id))
            {
                ModelState.AddModelError("Name", "A category with this name already exists.");
                return View(vm);
            }

            await _categoryService.UpdateCategoryAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vm = await _categoryService.GetCategoryByIdAsync(id.Value);
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
