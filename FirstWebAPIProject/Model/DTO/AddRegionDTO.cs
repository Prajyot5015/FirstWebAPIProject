using System.ComponentModel.DataAnnotations;

namespace FirstWebAPIProject.Model.DTO
{
    public class AddRegionDTO
    {
        [Required(ErrorMessage = "Region Code is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Code must be at least 3 characters")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Region name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be at least 3 characters")]
        public string Name { get; set; }

        public string? RegionImgUrl { get; set; }
    }
}
