using ShoppingCart.Models;

namespace ShoppingCart.Repository
{
    public class CartsUserDataRepo
    {
        public  static List<CartUsersModel> cartUser = new ()
        {
            new CartUsersModel
            {

                UserName="Michael",
                Password="Michael",
                Email="Michy@gmail.com",
                GivenName="MK",
                Role="Admin"

            },
             new CartUsersModel
            {

                UserName="Anabel",
                Password="Lucky",
                Email="Anna@gmail.com",
                GivenName="ALuckie",
                Role="User"

            },

              new CartUsersModel
            {

                UserName="Jojo",
                Password="JMK",
                Email="Jojo@gmail.com",
                GivenName="Joey",
                Role="User"

            },

        };
    }
}
