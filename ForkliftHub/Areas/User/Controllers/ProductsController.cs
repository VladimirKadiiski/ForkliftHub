using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForkliftHub.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class ProductsController(IProductService productService) : Controller
    {
        private readonly IProductService _productService = productService;

        public async Task<IActionResult> Index(
            string? searchTerm,
            int? categoryId,
            int? brandId,
            int? EngineId,
            int? MastTypeId,
            string? sortOrder,
            int page = 1)
        {
            var viewModel = await _productService.GetFilteredProductListAsync(
                searchTerm, categoryId, brandId, EngineId, MastTypeId, sortOrder, page, 10);

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var product = await _productService.GetProductDetailsAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }
    }
}
