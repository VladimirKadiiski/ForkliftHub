using System.ComponentModel.DataAnnotations;

namespace ForkliftHub.Models
{
    public class MastType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
