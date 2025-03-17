using FirstWebAPIProject.Model.Domain;

namespace FirstWebAPIProject.Repository.Interface
{
    public interface IRegionRepository
    {
        public Task<List<Region>> GetAllRegionAsync();

        public Task<Region?> GetRegionByIdAsync(Guid regionId);

        public Task<Region> AddRegionAsync(Region region);

        public Task<Region?> UpdateRegionAsync(Guid id , Region region);

        public Task<Region?> DeleteRegionAsync(Guid id);
    }
}
