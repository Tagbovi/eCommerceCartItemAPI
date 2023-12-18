using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetCartItems();
        Task<CartAndPriceItemTotal> GetTotalItemsAndTotalPrice();
        Task <Cart?> GetCartItemByID(int Id);
        Task<Cart> AddCartItem(Cart newcart);
        Task<Cart?> UpdateCartItem(Cart cartupdate);
        Task  DeleteCartItem(int id);
    }
}
