using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShoppingCart.Models;
using System.Security.Cryptography.X509Certificates;

namespace ShoppingCart.DBContext
{
    public class CartDBContext : DbContext
    {
        public CartDBContext(DbContextOptions<CartDBContext> options) :base(options) { }

        public DbSet<Cart> Cartdb { get; set; }
        public DbSet<CartAndPriceItemTotal> CartPriceTotaldb { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Cart>().HasData(
                new Cart
                {
                     
                      ProductId = 1,
                      Name = "Adidas Shoe",
                      Price= 200.00,
                      Quantity= 1,
                }
                
                );

            modelBuilder.Entity<CartAndPriceItemTotal>().HasData(
                new CartAndPriceItemTotal
                {
                    Id=1,
                    TotalCartItems=0,
                    TotalPrice=0,
                }

                );




        }
       


    }
}
