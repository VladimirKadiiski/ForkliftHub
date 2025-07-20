// ViewModels/ProductListViewModel.cs
using ForkliftHub.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ForkliftHub.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();

        public string? SearchTerm { get; set; }
        public string? SortOrder { get; set; }
        public int? SelectedCategoryId { get; set; }
        public int? SelectedBrandId { get; set; }
        public int? SelectedEngineId { get; set; }
        public int? SelectedMastTypeId { get; set; }

        public List<SelectListItem>? Categories { get; set; }
        public List<SelectListItem>? Brands { get; set; }
        public List<SelectListItem>? Engines { get; set; }
        public List<SelectListItem>? MastTypes { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

    }
}
