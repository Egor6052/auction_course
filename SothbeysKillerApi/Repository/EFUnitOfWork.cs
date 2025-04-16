using Microsoft.EntityFrameworkCore.Storage;
using SothbeysKillerApi.Context;
using System.Data;

namespace SothbeysKillerApi.Repository {
    public class EFUnitOfWork : IUnitOfWork {
        private readonly UserDBContext _userDBContext;
        private readonly IDbContextTransaction _transaction;

        public IUserRepository UserRepository { get; }
        public IAuctionRepository AuctionRepository { get; }

        public IAuctionHistoryRepository AuctionHistoryRepository => throw new NotImplementedException();

        // public IAuctionHistoryRepository AuctionHistoryRepository { get; }



        public EFUnitOfWork(UserDBContext userDBContext) {
            _userDBContext = userDBContext;

            _transaction = _userDBContext.Database.BeginTransaction();
            UserRepository = new EFUserRepository(_userDBContext.Users, _userDBContext);
            // AuctionRepository = new AuctionRepository(_userDBContext.Auctions, _userDBContext);
            // AuctionHistoryRepository = new EFAuctionHistoryRepository(_userDBContext.AuctionHistories, _userDBContext);
        }

        public void Commit() {
            try {
                _transaction.Commit();
            }
            catch (Exception e) {
                _transaction.Rollback();
            }
        }

        public void Rollback() {
            _transaction.Rollback();
        }
    }

}