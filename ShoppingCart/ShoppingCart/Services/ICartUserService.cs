using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public interface ICartUserService
    {
        Task< CartUsersModel> UserLogin(CartUserLogin login);
    }
}
