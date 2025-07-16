using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ForkliftHub.Models
{
    [Description("Individual Machine")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Name { get; set; } = null!;

        [Required]
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        [Required]
        public int ModelId { get; set; }
        public MachineModel Model { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Required]
        public int EngineId { get; set; }
        public Engine Engine { get; set; } = null!;

        [Required]
        public int MastTypeId { get; set; }
        public MastType MastType { get; set; } = null!;

        [Precision(18, 2)]
        [Range(0, 10000)]
        public decimal LiftingHeight { get; set; }

        [Precision(18, 2)]
        [Range(0, 10000)]
        public decimal ClosedHeight { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = null!;

        [Url]
        public string? ImageUrl { get; set; }

        [Precision(18, 2)]
        [Range(0.01, 1000000)]
        public decimal Price { get; set; }

        [Range(0, 10000)]
        public int Stock { get; set; }

        [Required]
        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } = null!;

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
