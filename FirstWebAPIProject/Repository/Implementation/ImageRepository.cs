using FirstWebAPIProject.Data;
using FirstWebAPIProject.Model.Domain;
using FirstWebAPIProject.Repository.Interface;

namespace FirstWebAPIProject.Repository.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment; // To get the root path of the project
        private readonly IHttpContextAccessor httpContextAccessor; // To get the current request URL
        private readonly AppDbContext appDbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, AppDbContext appDbContext)
        {
            this.webHostEnvironment = webHostEnvironment; 
            this.httpContextAccessor = httpContextAccessor;
            this.appDbContext = appDbContext;
        }
        public async Task<Image> Upload(Image image)
        {
            var fileNameWithExtension = image.FileName + image.FileExtension; 

            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", fileNameWithExtension);

            // Upload image to local folder 

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // https://localhost:1234/images/image.jpg

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            // Add this image to images table

            await appDbContext.Image.AddAsync(image);
            await appDbContext.SaveChangesAsync();

            return image;
        }
    }
}
