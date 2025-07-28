using ForkliftHub.Api.DTOs;
using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForkliftHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IProductService productService, ApplicationDbContext dbContext) : ControllerBase
    {
        private readonly IProductService _productService = productService;
        private readonly ApplicationDbContext _db = dbContext;

        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetFilteredProducts([FromBody] ProductFilterDto filter)
        {
            var vm = await _productService.GetFilteredProductListAsync(
                filter.SearchTerm,
                filter.CategoryId,
                filter.BrandId,
                filter.EngineId,
                filter.MastTypeId,
                filter.SortOrder,
                filter.Page,
                filter.PageSize);

            var productDtos = vm.Products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                LiftingHeight = (double?)p.LiftingHeight,
                ClosedHeight = (double?)p.ClosedHeight,
                ImageUrl = p.ImageUrl,
                BrandName = p.Brand?.Name ?? "",
                CategoryName = p.Category?.Name ?? "",
                ProductTypeName = p.ProductType?.Name ?? "",
                EngineType = p.Engine?.Type ?? "",
                MastTypeName = p.MastType?.Name ?? "",
                MachineModelName = p.MachineModel?.Name ?? ""
            });

            return Ok(new
            {
                vm.CurrentPage,
                vm.TotalPages,
                Products = productDtos
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductDetailsAsync(id);
            if (product == null) return NotFound();

            var dto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                LiftingHeight = (double?)product.LiftingHeight,
                ClosedHeight = (double?)product.ClosedHeight,
                ImageUrl = product.ImageUrl,
                BrandName = product.Brand?.Name ?? "",
                CategoryName = product.Category?.Name ?? "",
                ProductTypeName = product.ProductType?.Name ?? "",
                EngineType = product.Engine?.Type ?? "",
                MastTypeName = product.MastType?.Name ?? "",
                MachineModelName = product.MachineModel?.Name ?? ""
            };

            return Ok(dto);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                LiftingHeight = (decimal)dto.LiftingHeight,
                ClosedHeight = (decimal)dto.ClosedHeight,
                ImageUrl = dto.ImageUrl,
                BrandId = dto.BrandId,
                CategoryId = dto.CategoryId,
                ProductTypeId = dto.ProductTypeId,
                EngineId = dto.EngineId,
                MastTypeId = dto.MastTypeId,
                MachineModelId = dto.MachineModelId
            };

            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.LiftingHeight = (decimal)dto.LiftingHeight;
            product.ClosedHeight = (decimal)dto.ClosedHeight;
            product.ImageUrl = dto.ImageUrl;
            product.BrandId = dto.BrandId;
            product.CategoryId = dto.CategoryId;
            product.ProductTypeId = dto.ProductTypeId;
            product.EngineId = dto.EngineId;
            product.MastTypeId = dto.MastTypeId;
            product.MachineModelId = dto.MachineModelId;

            await _productService.UpdateProductAsync(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
