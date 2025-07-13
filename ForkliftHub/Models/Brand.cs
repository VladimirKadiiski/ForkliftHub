using System.ComponentModel.DataAnnotations;

namespace ForkliftHub.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        public ICollection<Model> Models { get; set; } = new List<Model>();
    }
}
