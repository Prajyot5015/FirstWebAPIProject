using FirstWebAPIProject.Data;
using FirstWebAPIProject.Model.Domain;
using FirstWebAPIProject.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FirstWebAPIProject.Repository.Implementation
{
    public class RegionRepository : IRegionRepository
    {
        private readonly AppDbContext _dbContext;

        public RegionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Region> AddRegionAsync(Region region)
        {
            await _dbContext.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteRegionAsync(Guid id)
        {
            var existingRegion = await _dbContext.Region.FindAsync(id);
            if (existingRegion == null) return null;

            _dbContext.Region.Remove(existingRegion);
            await _dbContext.SaveChangesAsync();
            return existingRegion;
        }

        public async Task<List<Region>> GetAllRegionAsync()
        {
            return await _dbContext.Region.ToListAsync();
        }

        public async Task<Region?> GetRegionByIdAsync(Guid regionId)
        {
            return await _dbContext.Region.FindAsync(regionId);
        }

        public async Task<Region?> UpdateRegionAsync(Guid id, Region region)
        {
            var existingRegionDomainModel = await _dbContext.Region.FindAsync(id);
            if (existingRegionDomainModel == null) return null;

            existingRegionDomainModel.Code = region.Code;
            existingRegionDomainModel.Name = region.Name;
            existingRegionDomainModel.RegionImgUrl = region.RegionImgUrl;

            await _dbContext.SaveChangesAsync();

            return existingRegionDomainModel;
        }
    }
}
