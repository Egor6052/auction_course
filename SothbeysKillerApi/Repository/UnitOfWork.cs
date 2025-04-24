using Microsoft.EntityFrameworkCore;
using SothbeysKillerApi.Context;

namespace SothbeysKillerApi.Repository
{
    public class EFUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly UserDBContext _context;
        private bool _disposed;
        private IUserRepository _userRepository;
        private IAuctionRepository _auctionRepository;
        private IAuctionHistoryRepository _auctionHistoryRepository;

        public EFUnitOfWork(UserDBContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository => 
            _userRepository ??= new EFUserRepository(_context);

        public IAuctionRepository AuctionRepository => 
            _auctionRepository ??= new EFAuctionRepository(_context);

        public IAuctionHistoryRepository AuctionHistoryRepository => 
            _auctionHistoryRepository ??= new EFAuctionHistoryRepository(_context);

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RollbackAsync()
        {
            // Автоматичний відкат змін, якщо SaveChanges не викликано
            await _context.Database.CurrentTransaction?.RollbackAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}