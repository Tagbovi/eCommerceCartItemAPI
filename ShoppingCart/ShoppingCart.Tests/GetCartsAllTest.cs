using AutoMapper;
using FakeItEasy;
using FakeItEasy.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Controllers;
using ShoppingCart.Models;
using ShoppingCart.Services;
using System.Xml.Linq;


namespace ShoppingCart.Tests
{
    public class CartControllerTest1
    {
        [Fact]  
        public async Task GetAllCarts_ReturnsOkResult()
        {
            // Arrange
            var cartService = A.Fake<ICartService>();
            A.CallTo(() => cartService.GetCartItems()).Returns(new List<Cart>());

            var mapper = A.Fake<IMapper>(); 

           
            var logger = A.Fake<LoggingError.ILoggerWrapper>();
            var controller = new CartController(cartService, mapper, logger);

            // Act
            var result = await controller.GetAllCartItems();

            // Assert
            A.CallTo(() => cartService.GetCartItems()).MustHaveHappenedOnceExactly();
            Assert.IsType<OkObjectResult>(result.Result);
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        }
    }
    
}
