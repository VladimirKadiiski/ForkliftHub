
using System.ComponentModel.DataAnnotations;

namespace ForkliftHub.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}


namespace ForkliftHub.ViewModels
{
    public class CategoryDeleteViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

