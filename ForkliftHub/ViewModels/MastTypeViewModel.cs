using System.ComponentModel.DataAnnotations;

namespace ForkliftHub.ViewModels
{
    public class MastTypeViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
