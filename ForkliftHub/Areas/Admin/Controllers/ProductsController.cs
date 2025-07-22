using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForkliftHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController(IProductService productService) : Controller
    {
        private readonly IProductService _productService = productService;

        public async Task<IActionResult> Index(string? searchTerm, int? categoryId, int? brandId, int? engineId, int? mastTypeId, string? sortOrder, int page = 1)
        {
            var viewModel = await _productService.GetFilteredProductListAsync(
                searchTerm, categoryId, brandId, engineId, mastTypeId, sortOrder, page, 10);

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _productService.GetProductDetailsAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            var formVm = await _productService.GetProductFormViewModelAsync();
            return View(formVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await _productService.GetProductFormViewModelAsync();
                return View(viewModel);
            }

            await _productService.CreateProductAsync(viewModel);
            TempData["SuccessMessage"] = "Product created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var formVm = await _productService.GetProductFormViewModelAsync(id);
            if (formVm == null) return NotFound();

            return View(formVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductFormViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                viewModel = await _productService.GetProductFormViewModelAsync(viewModel.Id);
                return View(viewModel);
            }

            var updated = await _productService.UpdateProductAsync(viewModel);
            if (!updated) return NotFound();

            TempData["SuccessMessage"] = "Product updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _productService.GetProductDetailsAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProductAsync(id);
            TempData["SuccessMessage"] = "Product deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
