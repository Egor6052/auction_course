using Dapper;
using Npgsql;
using SothbeysKillerApi.Controllers;
using System.Data;

namespace SothbeysKillerApi.Repository
{
    public class DbUserRepostory : IUserRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _transaction;
        public DbUserRepostory(IDbConnection dbConnection, IDbTransaction transaction)
        {

            _dbConnection = dbConnection;

            _transaction = transaction;

            _dbConnection.Open();

        }

        public bool EmailExist(string email)
        {
            var query = "select exists(select * from Users where email = @Email)";
            var answer = _dbConnection.ExecuteScalar<bool>(query, new { Email = email });
            return answer;
        }
        public void Create(User entity)
        {
            var command = $@"insert into users (id, name, email, password) values (@Id, @Name, @Email, @Password);";
            _dbConnection.ExecuteScalar(command, entity);
        }

        public User? Signin(string email)
        {
            var query = "select * from users where email = @Email";
            var user = _dbConnection.QuerySingleOrDefault<User>(query, new { Email = email });
            return user;
        }
    }
}