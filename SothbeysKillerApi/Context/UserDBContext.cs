using Microsoft.EntityFrameworkCore;
using SothbeysKillerApi.Controllers;

namespace SothbeysKillerApi.Context
{
    public class UserDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }

        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Налаштування первинних ключів
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Auction>().HasKey(a => a.Id);

            // Налаштування обмежень для Auction
            modelBuilder.Entity<Auction>()
                .Property(a => a.Title)
                .HasMaxLength(255)
                .IsRequired();

        }
    }
}