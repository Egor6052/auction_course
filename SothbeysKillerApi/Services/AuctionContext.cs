// // using Microsoft.EntityFrameworkCore;
// using SothbeysKillerApi.Controllers;
// // using SothbeysKillerApi.Models;

// namespace SothbeysKillerApi.Data;

// public class AuctionContext : DbContext
// {
//     public AuctionContext(DbContextOptions<AuctionContext> options)
//         : base(options)
//     {
//     }

//     public DbSet<Auction> Auctions { get; set; } = null!;

//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         modelBuilder.Entity<Auction>().HasKey(a => a.Id);
//         modelBuilder.Entity<User>().HasKey(u => u.Id);

//         modelBuilder.Entity<Auction>()
//             .HasOne(a => a.Seller)
//             .WithMany(u => u.Auctions)
//             .HasForeignKey(a => a.SellerId);

//         modelBuilder.Entity<Auction>()
//             .Property(a => a.StartingPrice)
//             .HasColumnType("decimal(18,2)");
//     }
// }
