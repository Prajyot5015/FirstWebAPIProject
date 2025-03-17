using FirstWebAPIProject.Data;
using FirstWebAPIProject.Model.Domain;
using FirstWebAPIProject.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FirstWebAPIProject.Repository.Implementation
{
    public class WalksRepository : IWalksRepository
    {
        private readonly AppDbContext _appDbContext;
        public WalksRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Walk> AddWalkAsync(Walk walk)
        {
            await _appDbContext.Walk.AddAsync(walk);
            await _appDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteWalkAsync(Guid id)
        {
            var existingWalk = await _appDbContext.Walk.FindAsync(id);
            if (existingWalk == null) return null;

            _appDbContext.Walk.Remove(existingWalk);
            await _appDbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks = _appDbContext.Walk.Include("Difficulty").Include("Region").AsQueryable();

            // Filtering
            if(string.IsNullOrEmpty(filterOn) == false && string.IsNullOrEmpty(filterQuery) == false)
            {
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            // Sorting

            if(string.IsNullOrEmpty(sortBy) == false )
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if(sortBy.Equals("Length",StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x=>x.LengthInKm);
                }
            }

            // Pagination

            var skipResult = (pageNumber - 1)  * pageSize;



            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();
           // return await _appDbContext.Walk.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid id)
        {
            return await _appDbContext.Walk
                                       .Include("Difficulty")
                                       .Include("Region")
                                       .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateWalkAsync(Guid id, Walk walk)
        {
            var existingWalk = await _appDbContext.Walk.FindAsync(id);

            if (existingWalk == null) return null;

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImgUrl = walk.WalkImgUrl;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;

            await _appDbContext.SaveChangesAsync();

            return existingWalk;
        }
    }
}
