
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ForkliftHub.ViewModels
{
    public class MachineModelViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Model Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Brand")]
        public int BrandId { get; set; }

        // For dropdown population
        public List<SelectListItem>? Brands { get; set; }

    }
}


namespace ForkliftHub.ViewModels
{
    public class MachineModelDeleteViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string BrandName { get; set; } = string.Empty;
    }
}

