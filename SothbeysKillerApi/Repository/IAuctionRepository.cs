using SothbeysKillerApi.Controllers;
// using SothbeysKillerApi.Models;

namespace SothbeysKillerApi.Repository;

public interface IAuctionRepository {
    Task<IEnumerable<Auction>> GetPastAsync();
    Task<IEnumerable<Auction>> GetActiveAsync();
    Task<IEnumerable<Auction>> GetFutureAsync();
    Task<Auction?> GetByIdAsync(Guid id);
    Task<Auction> CreateAsync(Auction entity);
    Task<Auction?> UpdateAsync(Auction entity);
    Task DeleteAsync(Guid id);
}
