using Microsoft.EntityFrameworkCore;
using SothbeysKillerApi.Context;
using SothbeysKillerApi.Controllers;

namespace SothbeysKillerApi.Repository
{
    public class EFAuctionRepository : IAuctionRepository
    {
        private readonly UserDBContext _context;

        public EFAuctionRepository(UserDBContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Auction entity)
        {
            await _context.Auctions.AddAsync(entity);
        }

        public async Task UpdateAsync(Auction entity)
        {
            _context.Auctions.Update(entity);
        }

        public async Task DeleteAsync(Auction entity)
        {
            _context.Auctions.Remove(entity);
        }

        public async Task<Auction?> GetByIdAsync(Guid id)
        {
            return await _context.Auctions.FindAsync(id);
        }

        public async Task<IEnumerable<Auction>> GetPastAsync()
        {
            return await _context.Auctions
                .Where(a => a.Finish < DateTime.Now)
                .OrderByDescending(a => a.Start)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetActiveAsync()
        {
            return await _context.Auctions
                .Where(a => a.Start < DateTime.Now && a.Finish > DateTime.Now)
                .OrderByDescending(a => a.Start)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetFutureAsync()
        {
            return await _context.Auctions
                .Where(a => a.Start > DateTime.Now)
                .OrderByDescending(a => a.Start)
                .ToListAsync();
        }
    }
}