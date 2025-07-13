using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ForkliftHub.Models
{
    [Description("Type of mast of machine")]
    public class MastType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
