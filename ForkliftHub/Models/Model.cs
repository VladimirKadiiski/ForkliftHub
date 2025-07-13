using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ForkliftHub.Models
{
    [Description("Model of machine")]
    public class Model
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
