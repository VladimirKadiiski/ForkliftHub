using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ForkliftHub.Models
{
    [Description("Type of the product")]
    public class ProductType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;
    }
}
