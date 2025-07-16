using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForkliftHub.Models
{
    [Table("MachineModels")]
    [Description("Model of machine")]
    public class MachineModel
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
