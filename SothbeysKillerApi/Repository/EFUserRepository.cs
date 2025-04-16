using Microsoft.EntityFrameworkCore;
using SothbeysKillerApi.Controllers;
using SothbeysKillerApi.Infrastructure;

namespace SothbeysKillerApi.Repository {
    public class EFUserRepository : IUserRepository {
        private readonly DbSet<User> _dbSet;
        private readonly IChangeSaver _context;

        public EFUserRepository(DbSet<User> dbSet, IChangeSaver context) {
            _dbSet = dbSet;
            _context = context;
        }
        public void Create(User entity) {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public bool EmailExist(string email) {
            return _dbSet.Any(u => u.Email == email); ;
        }

        public User? Signin(string email) {
            return _dbSet.SingleOrDefault(u => u.Email == email);
        }
    }
}