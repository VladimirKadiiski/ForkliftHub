using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ForkliftHub.Models
{
    [Description("Engine type of machine")]
    public class Engine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Type { get; set; } = null!;
    }
}
