using System.ComponentModel.DataAnnotations;

namespace FirstWebAPIProject.Model.DTO
{
    public class UpdateWalkDTO
    {
        [Required(ErrorMessage = "Walk name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be at least 3 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Walk Description is required")]
        [StringLength(1000, MinimumLength = 20, ErrorMessage = "Description must be at least 20 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Walk Length is required")]
        public double LengthInKm { get; set; }

        public string? WalkImgUrl { get; set; }

        [Required(ErrorMessage = "Walk Difficulty Id is required")]
        public Guid DifficultyId { get; set; }

        [Required(ErrorMessage = "Walk Region Id is required")]
        public Guid RegionId { get; set; }
    }
}
