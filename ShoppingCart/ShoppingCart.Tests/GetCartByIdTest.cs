using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Controllers;
using ShoppingCart.LoggingError;
using ShoppingCart.Models;
using ShoppingCart.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Tests
{
    public class GetCartByIdTest
    {
        [Fact]
        public async Task GetCartItemById_ReturnsOkObjectResult_WhenCartItemExists()
        {
            // Arrange
            var id = 1;
            var fakeCartService = A.Fake<ICartService>(); 
            var fakeMapper = A.Fake<IMapper>();
           
            var fakeLogger = A.Fake<ILoggerWrapper>();
            var controller = new CartController(fakeCartService, fakeMapper, fakeLogger);

            // Mock the behavior of the cart service
            A.CallTo(() => fakeCartService.GetCartItemByID(id)).Returns(new Cart { 
             ProductId=id,
             Name="Adidas",
             Price=203,
             Quantity=2,
             
            
            });

            // Act
            var result = await controller.GetCartItemById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var cartItem = Assert.IsType<Cart>(okResult.Value);
            Assert.NotNull(cartItem);

            A.CallTo(() => fakeCartService.GetCartItemByID(id)).MustHaveHappenedOnceExactly(); // Verify that the method was called once
        }

        [Fact]
        public async Task GetCartItemById_ReturnsNotFound_WhenCartItemDoesNotExist()
        {
            // Arrange
            var id = 1;
            var fakeCartService = A.Fake<ICartService>(); // Assuming ICartService is the interface for your cart service
            var fakeMapper = A.Fake<IMapper>();
           // var fakeLogger = A.Fake<ILogger<CartController>>();
            var fakeLogger = A.Fake<ILoggerWrapper>();
            var controller = new CartController(fakeCartService, fakeMapper, fakeLogger);

            // Mock the behavior of the cart service
            A.CallTo(() => fakeCartService.GetCartItemByID(id)).Returns(Task.FromResult<Cart?>(null));

            // Act
            var result = await controller.GetCartItemById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);

            A.CallTo(() => fakeCartService.GetCartItemByID(id)).MustHaveHappenedOnceExactly(); // Verify that the method was called once
        }

        [Fact]
        public async Task GetCartItemById_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var id = 1;
            var fakeCartService = A.Fake<ICartService>(); // Assuming ICartService is the interface for your cart service
            var fakeMapper = A.Fake<IMapper>();
           // var fakeLogger = A.Fake<ILogger<CartController>>();
            var fakeLogger = A.Fake<ILoggerWrapper>();
            var controller = new CartController(fakeCartService, fakeMapper, fakeLogger);

            // Mock the behavior of the cart service to throw an exception
            A.CallTo(() => fakeCartService.GetCartItemByID(id)).Throws<Exception>();

            // Act
            var result = await controller.GetCartItemById(id);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);

            A.CallTo(() => fakeCartService.GetCartItemByID(id)).MustHaveHappenedOnceExactly(); // Verify that the method was called once
            A.CallTo(() => fakeLogger.LogError(A<Exception>._, A<string>._, A<object[]>._)).MustHaveHappenedOnceExactly(); // Verify that the logger was called once
        }
    }
}
