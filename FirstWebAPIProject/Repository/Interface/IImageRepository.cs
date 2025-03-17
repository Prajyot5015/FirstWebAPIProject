using FirstWebAPIProject.Model.Domain;

namespace FirstWebAPIProject.Repository.Interface
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
