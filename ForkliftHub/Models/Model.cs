using System.ComponentModel.DataAnnotations;

namespace ForkliftHub.Models
{
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
    }
}
