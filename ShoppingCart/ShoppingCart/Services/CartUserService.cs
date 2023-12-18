using ShoppingCart.Repository;
using ShoppingCart.Models;


namespace ShoppingCart.Services
{
    public class CartUserService : ICartUserService
    {
       
       
        public Task< CartUsersModel> UserLogin(CartUserLogin login)
        {
            return Task.Run(() => {

                var result = CartsUserDataRepo.cartUser.FirstOrDefault(e =>
               e.UserName.Equals(login.UserName) && e.Password.Equals(login.Password));
               
                return result != null ? Task.FromResult(result) : Task.FromResult<CartUsersModel>(null);
            });
           
        }
    }
}
