using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DTOs;
using ShoppingCart.LoggingError;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly IMapper _mapper;
        // private readonly ILogger<CartController> _logger;
        private readonly ILoggerWrapper _logger;


        public CartController(ICartService _cartService, IMapper _mapper,ILoggerWrapper logger
           
            )
        { this.cartService = _cartService;
        this._mapper = _mapper; 
            this._logger = logger;
            
        }





        /// <summary>
        /// Gets all Carts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles ="User,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Cart>>> GetAllCartItems()
        {
            try
            {
                return Ok(await cartService.GetCartItems());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllCartItems)}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Please try again later");
            }

        }

        /// <summary>
        /// Gets total number of CartItems and TotalPrice of all Items
        /// </summary>
        /// <returns></returns>

        [HttpGet("totalItems&totalPrice")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetCartAndPriceItemsDTO>> GetTotalItemsAndTotalPrice()
        {
            try
            {
                var cartItems = await cartService.GetTotalItemsAndTotalPrice();
                
                    return Ok(_mapper.Map<GetCartAndPriceItemsDTO>(cartItems));
                
               

            }
            catch (Exception ex){
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetTotalItemsAndTotalPrice)}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Please try again later");
            }
        }
       
        /// <summary>
        /// Creates a new CartItem
        /// </summary>
        /// <param name="newcart"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Cart>> AddCartItem(Cart newcart)
        {
            try
            {
                if (newcart.Price == 0 && newcart.Quantity == 0)
                {
                    return BadRequest("Price and Quantity field cannot be empty ");
                }

                if (newcart.Name==null)
                {
                    return BadRequest("Name field cannot be empty ");
                }
                if (newcart.Price == 0)
                {
                    return BadRequest("Price field cannot be empty ");
                }
                if (newcart.Quantity == 0)
                {
                    return BadRequest("Quantity field cannot be empty ");
                }
              
                     var cartModel = await cartService.AddCartItem(newcart);
                
                    return CreatedAtAction(nameof(GetCartItemById), new { id = newcart.ProductId }, cartModel);
                   
                
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(AddCartItem)}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Please try again later");
            }

        }

        /// <summary>
        /// Returns CartItem by a given Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Cart>> GetCartItemById(int id)
        
        {
            try
            {
                var cartItem= await cartService.GetCartItemByID(id);
                if (cartItem != null)
                {
                    return Ok(cartItem);
                }
                return NotFound() ;
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, $"Something went wrong in the {nameof(GetCartItemById)}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Please try again later");
            }

        }

        /// <summary>
        /// Updates CartItem by Id
        /// </summary>
        /// <param name="cart"></param>
       
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Cart>> UpdateCartItem(Cart cart)
        {
            try
            {
               

                var cartid = await cartService.GetCartItemByID(cart.ProductId);

                if (cartid == null)
                {
                    return NotFound($"Cart Item with that Id  does not exist");
                }
                if(cart.Price==0 && cart.Quantity == 0)
                {
                    return BadRequest("Price and Quantity cannot be empty");
                }
                if(cart.Price == 0)
                {
                    return BadRequest("Price cannot be empty");
                }
                if (cart.Quantity == 0)
                {
                    return BadRequest("quantity cannot be empty");
                }

                var cartItemToUpdate = await cartService.UpdateCartItem(cart);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateCartItem)}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Please try again later");

            }

        }


        /// <summary>
        /// Deletes cart Item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCartItem(int id)
        {
            try
            {
                var cartitem = await cartService.GetCartItemByID(id);
                if (cartitem == null)
                {
                    return NotFound($"CartItem with {id} does not exist");

                }
                await cartService.DeleteCartItem(id);
                return Ok($"Cart Item successfully deleted");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteCartItem)}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Please try again later");

            }



        }


    }
}
