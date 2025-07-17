using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ForkliftHub.ViewModels
{
    public class ProductFormViewModel
    {
        public int? Id { get; set; } // null for Create, set for Edit

        [Required, StringLength(120)]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Brand")]
        public int BrandId { get; set; }

        [Required]
        [Display(Name = "Model")]
        public int MachineModelId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Engine")]
        public int EngineId { get; set; }

        [Required]
        [Display(Name = "Mast Type")]
        public int MastTypeId { get; set; }

        [Range(0, 10000)]
        public decimal LiftingHeight { get; set; }

        [Range(0, 10000)]
        public decimal ClosedHeight { get; set; }

        [Required, StringLength(1000)]
        public string Description { get; set; } = null!;

        [Url]
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [Range(0.01, 1000000)]
        public decimal Price { get; set; }

        [Range(0, 10000)]
        public int Stock { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }

        // Dropdowns
        public SelectList? Brands { get; set; }
        public SelectList? MachineModels { get; set; }
        public SelectList? Categories { get; set; }
        public SelectList? Engines { get; set; }
        public SelectList? MastTypes { get; set; }
        public SelectList? ProductTypes { get; set; }
    }
}
