
using AutoMapper;
using ShoppingCart.DTOs;
using ShoppingCart.Models;

namespace ShoppingCart.Profiles
{
 public class CartProfile : Profile
    { 
        public CartProfile() {

            CreateMap<CartAndPriceItemTotal, GetCartAndPriceItemsDTO>();
        }
    
    
    
    
    }

}
