using FirstWebAPIProject.Model.Domain;

namespace FirstWebAPIProject.Repository.Interface
{
    public interface IWalksRepository
    {
        Task<Walk> AddWalkAsync(Walk walk);

        Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null, 
            string? sortBy =null, bool isAscending = true,
             int pageNumber = 1, int pageSize = 1000);

        Task<Walk?> GetWalkByIdAsync(Guid id);

        Task<Walk?> UpdateWalkAsync(Guid id, Walk walk);

        Task<Walk?> DeleteWalkAsync(Guid id);
    }
}
