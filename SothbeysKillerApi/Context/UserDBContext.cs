using Microsoft.EntityFrameworkCore;
using SothbeysKillerApi.Controllers;
using SothbeysKillerApi.Infrastructure;

namespace SothbeysKillerApi.Context
{
    public class UserDBContext : DbContext, IChangeSaver
    {
        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

    }
}