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
    public class AddCartItemsTest
    {
      
        
            [Fact]
            public async Task AddCartItem_ReturnsCreated_WhenServiceSucceeds()
            {
                // Arrange
                var fakeCartService = A.Fake<ICartService>();
                 var fakeLogger = A.Fake<ILoggerWrapper>();
                 var fakeMapper = A.Fake<IMapper>();

                var controller = new CartController(fakeCartService,fakeMapper, fakeLogger);

                var newCart = new Cart
                {
                    ProductId = 1,
                    Name = "PopCornChips",
                    Price = 10.0,
                    Quantity = 5
                };

                var fakeAddedCart = new Cart
                {
                  
                    ProductId = 1,
                    Name = "PopCornChips",
                    Price = 10.0,
                    Quantity = 5
                };

                A.CallTo(() => fakeCartService.AddCartItem(newCart)).Returns(Task.FromResult(fakeAddedCart));

                // Act
                var result = await controller.AddCartItem(newCart);

            // Assert
             var actionResult = Assert.IsType<ActionResult<Cart>>(result);
             var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result); // Check the Result property of ActionResult

             Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
             Assert.Equal(nameof(controller.GetCartItemById), createdResult.ActionName);

             Assert.Equal(new[] { new KeyValuePair<string, object>("id", newCart.ProductId) }, createdResult.RouteValues);

           
             A.CallTo(() => fakeCartService.AddCartItem(newCart)).MustHaveHappenedOnceExactly();
            }

            [Theory]
            [InlineData(null, 10.0, 5, "Name field cannot be empty ")]
            [InlineData("PopCornChips", 0, 5, "Price field cannot be empty ")]
            [InlineData("PopCornChips", 10.0, 0, "Quantity field cannot be empty ")]
            [InlineData("TestPopCornChips", 0, 0, "Price and Quantity field cannot be empty ")]
            public async Task AddCartItem_ReturnsBadRequest_WhenValidationFails(string name, double price, int quantity, string expectedErrorMessage)
            {
                // Arrange
                var fakeCartService = A.Fake<ICartService>();
                var fakeLogger = A.Fake<ILoggerWrapper>();
                 var fakeMapper = A.Fake<IMapper>();

                 var controller = new CartController(fakeCartService, fakeMapper,fakeLogger);
                

            var newCart = new Cart
                {
                    ProductId = 1,
                    Name = name,
                    Price = price,
                    Quantity = quantity
                };

                // Act
                var result = await controller.AddCartItem(newCart);

            // Assert
             var actionResult = Assert.IsType<ActionResult<Cart>>(result);

             var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
                Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
                Assert.Equal(expectedErrorMessage, badRequestResult.Value);

                A.CallTo(() => fakeCartService.AddCartItem(A<Cart>._)).MustNotHaveHappened(); // Ensure that the service method is not called
            }

        [Fact]
        public async Task AddCartItem_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var fakeCartService = A.Fake<ICartService>();
            var fakeLogger = A.Fake<ILoggerWrapper>();
            var fakeMapper = A.Fake<IMapper>();
            var controller = new CartController(fakeCartService, fakeMapper, fakeLogger);

            var newCart = new Cart
            {
                ProductId = 1,
                Name = "TestProduct",
                Price = 10.0,
                Quantity = 5
            };

            A.CallTo(() => fakeCartService.AddCartItem(newCart)).Throws<Exception>();

            // Act
            var result = await controller.AddCartItem(newCart);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Cart>>(result);

            // Check if the ActionResult has the expected type
            Assert.IsType<ObjectResult>(actionResult.Result); // Check the Result property of ActionResult

            var statusCodeResult = (ObjectResult)actionResult.Result;
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);

            A.CallTo(() => fakeCartService.AddCartItem(newCart)).MustHaveHappenedOnceExactly(); // Verify that the method was called once
            A.CallTo(() => fakeLogger.LogError(A<Exception>._, A<string>._)).MustHaveHappenedOnceExactly(); // Verify that the logger was called once
        }

    }
}

