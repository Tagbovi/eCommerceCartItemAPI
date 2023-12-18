using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ShoppingCart.LoggingError;
using ShoppingCart.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoppingCart.Services
{
    public class TokenGeneratorService 
    {
        private readonly ILoggerWrapperLogin logger;
       

         private readonly IConfiguration configuration;




        
        public TokenGeneratorService(ILoggerWrapperLogin logger, IConfiguration configuration)
        {
            
         this.logger=logger;
            this.configuration = configuration;

        }

        public string Generate(CartUsersModel user) 
        {
            try
            {
               
                
                var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

                var claim = new[] {
                    new Claim(ClaimTypes.NameIdentifier,user.UserName ),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.GivenName, user.GivenName ),
                     new Claim(ClaimTypes.Role,user.Role),

                };

                var jwttoken = new JwtSecurityToken(
                    configuration["Jwt:Issuer"],
                    configuration["Jwt:Audience"],
                    claim,
                    expires: DateTime.UtcNow.AddDays(10),
                    signingCredentials: signingCredentials
                );

                return new JwtSecurityTokenHandler().WriteToken(jwttoken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"there was porblem witth {(nameof(user.UserName))}");
                throw;

            }
        }

    }
}
