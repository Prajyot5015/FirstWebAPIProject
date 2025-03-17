using System.ComponentModel.DataAnnotations;

namespace FirstWebAPIProject.Model.DTO
{
    public class ImageUploadDTO
    {
        [Required]

        public IFormFile File { get; set; }

        [Required]
        public string FileName { get; set; }

        public string? Description { get; set; }
    }
}
