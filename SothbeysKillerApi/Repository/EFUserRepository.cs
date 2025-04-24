using Microsoft.EntityFrameworkCore;
using SothbeysKillerApi.Context;
using SothbeysKillerApi.Controllers;

namespace SothbeysKillerApi.Repository
{
    public class EFUserRepository : IUserRepository
    {
        private readonly UserDBContext _context;

        public EFUserRepository(UserDBContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
        }

        public async Task<bool> EmailExistAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> SigninAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}