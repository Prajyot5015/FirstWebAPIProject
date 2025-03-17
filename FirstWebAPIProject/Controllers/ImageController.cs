using FirstWebAPIProject.Model.Domain;
using FirstWebAPIProject.Model.DTO;
using FirstWebAPIProject.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]

        public async Task<IActionResult> Upload([FromForm] ImageUploadDTO imageUploadDTO)
        {
            ValidateFileUpload(imageUploadDTO);

            if(ModelState.IsValid)
            {
                // Covert DTO to Domain Model

                var imageDomainModel = new Image
                {
                    File = imageUploadDTO.File,
                    FileExtension = Path.GetExtension(imageUploadDTO.File.FileName),
                    FileSizeInBytes = imageUploadDTO.File.Length,
                    FileName = imageUploadDTO.File.FileName,
                    FileDescription = imageUploadDTO.Description
                };

                // User repo to add Image

                await imageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);

            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadDTO imageUploadDTO)
        {
            var allowExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if(!allowExtensions.Contains(Path.GetExtension(imageUploadDTO.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported File Extention");
            }

            if(imageUploadDTO.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File Size more than 10MB, Please upload a smaller size file");
            }
        }
    }
}
