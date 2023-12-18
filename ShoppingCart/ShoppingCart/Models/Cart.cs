using ShoppingCart.DBContext;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models
{
    public class Cart
    {
        //ID, name, price, and quantity. ,total items, total price
      
        [Key]
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set;}
    }
}
