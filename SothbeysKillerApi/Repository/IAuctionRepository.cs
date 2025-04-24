using SothbeysKillerApi.Controllers;

namespace SothbeysKillerApi.Repository
{
    public interface IAuctionRepository
    {
        Task<IEnumerable<Auction>> GetPastAsync();
        Task<IEnumerable<Auction>> GetActiveAsync();
        Task<IEnumerable<Auction>> GetFutureAsync();
        Task<Auction?> GetByIdAsync(Guid id);
        Task CreateAsync(Auction entity);
        Task UpdateAsync(Auction entity);
        Task DeleteAsync(Auction entity);
    }
}