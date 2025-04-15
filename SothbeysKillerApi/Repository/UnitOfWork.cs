using Npgsql;
using System.Data;

namespace SothbeysKillerApi.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _transaction;

        public IUserRepository UserRepository { get; }

        public UnitOfWork()
        {
            _dbConnection = new NpgsqlConnection("Server=localhost;Port=5432;Database=auction_db;Username=postgres;Password=123456");
            _dbConnection.Open();

            _transaction = _dbConnection.BeginTransaction();

            UserRepository = new DbUserRepostory(_dbConnection, _transaction);
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch (Exception e)
            {
                _transaction.Rollback();
            }
            finally
            {
                _transaction.Dispose();
                _dbConnection.Dispose();
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            try
            {
                _transaction.Rollback();
                _transaction.Dispose();
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Transaction already disposed.");
            }

            _dbConnection.Dispose();
        }
    }
}