using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.DBContext;
using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public class CartService : ICartService
    {
        private readonly CartDBContext cartDBContext;

        public CartService(CartDBContext _cartDbContext) { this.cartDBContext = _cartDbContext; }
        public async Task<Cart> AddCartItem(Cart newcart)
        {
            if (newcart.Quantity == 1) {
                var result = cartDBContext.Cartdb.Add(newcart);
                await cartDBContext.SaveChangesAsync();
                return result.Entity;
            }
            
           if(newcart.Quantity > 1)
            {
                newcart.Price = newcart.Price * newcart.Quantity;

                new Cart { Price = newcart.Price };
                
               
            }
            var result2= cartDBContext.Update(newcart);
            await cartDBContext.SaveChangesAsync();

            return result2.Entity;

        }

        public async Task DeleteCartItem(int id)
        {
            var existingcart= await cartDBContext.Cartdb.FirstOrDefaultAsync(e=>e.ProductId==id);
            if (existingcart!=null) { 
              cartDBContext.Cartdb.Remove(existingcart);
                await cartDBContext.SaveChangesAsync();
                
            }
        }

        public async Task<Cart?> GetCartItemByID(int Id)
        {
            var cartbyId = await cartDBContext.Cartdb.FirstOrDefaultAsync(e => e.ProductId == Id);
            if (cartbyId != null)
            {
                return cartbyId;
            }
            return null;
        }

        public async Task<IEnumerable<Cart>> GetCartItems()
        {
            return await cartDBContext.Cartdb.ToListAsync();


        }




        public async Task<Cart?> UpdateCartItem(Cart cartupdate)
        {
            var cartId = await cartDBContext.Cartdb.FirstOrDefaultAsync(e => e.ProductId == cartupdate.ProductId);
            if(cartId != null)
            {
                cartId.ProductId= cartupdate.ProductId;
                cartId.Name=cartupdate.Name;
                if (cartupdate.Price != 0)
                {
                    cartId.Price = cartupdate.Price;
                }
                if(cartupdate.Quantity != 0)
                {
                    cartId.Quantity = cartupdate.Quantity;
                }
                
                await cartDBContext.SaveChangesAsync();
                return cartId;
            }
            return null;

        }

      
        public async Task<CartAndPriceItemTotal> GetTotalItemsAndTotalPrice()
        {
            IQueryable<Cart> query1 = cartDBContext.Cartdb;
            IQueryable<CartAndPriceItemTotal> query = cartDBContext.CartPriceTotaldb;

            int totalCartItems = await query1.CountAsync();
            
            
            double totalPrice = await query1.SumAsync(e=>e.Price );

           

            return new CartAndPriceItemTotal { TotalCartItems = totalCartItems, TotalPrice = totalPrice};
        }

    }
}
