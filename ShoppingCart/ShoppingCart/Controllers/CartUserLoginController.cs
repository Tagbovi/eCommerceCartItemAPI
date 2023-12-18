using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShoppingCart.LoggingError;
using ShoppingCart.Models;
using ShoppingCart.Services;
using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartUserLoginController : ControllerBase
    {
        
        private readonly ILoggerWrapperLogin logger;
        private readonly ICartUserService cartUserService;
        private readonly IConfiguration configuration;

        public CartUserLoginController(ICartUserService _cartUserService,
           ILoggerWrapperLogin logger, IConfiguration configuration) 
        { this.cartUserService = _cartUserService; this.logger = logger;
           this.configuration = configuration;
         
        }



        /// <summary>
        /// Login  used to generate Jwt tokens
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Generate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task< IActionResult> Login(CartUserLogin login)
        {
           
            try
            {
                TokenGeneratorService generateTokenService1 = new TokenGeneratorService(logger,configuration) ;
                
                var user= await cartUserService.UserLogin(login);
                if(user != null)
                {

                    var token = generateTokenService1.Generate(user);
                    return Ok(token);
                }
                return NotFound("Cart User cannot be found");
            }
            catch (Exception ex) {
                logger.LogError(ex, $"Something went wrong in the {nameof(Login)}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Please try again later");
            }
        }





    }
}
