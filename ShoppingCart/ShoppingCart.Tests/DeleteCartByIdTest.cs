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
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCart.Tests
{
    public class DeleteCartItemTest
    {
        [Fact]
        public async Task DeleteCartItem_ReturnsOk_WhenItemExists()
        {
            // Arrange
            var fakeCartService = A.Fake<ICartService>();
            var fakeLogger = A.Fake<ILoggerWrapper>();
            var fakeMapper = A.Fake<IMapper>();
            var controller = new CartController(fakeCartService, fakeMapper, fakeLogger);

            var existingCart = new Cart
            {
                ProductId = 1,
                Name = "ExistingProduct",
                Price = 10.0,
                Quantity = 5
            };

            A.CallTo(() => fakeCartService.GetCartItemByID(existingCart.ProductId)).Returns(Task.FromResult<Cart?>(existingCart));

            // Act
            var result = await controller.DeleteCartItem(existingCart.ProductId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("Cart Item successfully deleted", okResult.Value);

            A.CallTo(() => fakeCartService.GetCartItemByID(existingCart.ProductId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeCartService.DeleteCartItem(existingCart.ProductId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteCartItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            var fakeCartService = A.Fake<ICartService>();
            var fakeLogger = A.Fake<ILoggerWrapper>();
            var fakeMapper = A.Fake<IMapper>();
            var controller = new CartController(fakeCartService, fakeMapper, fakeLogger);

            A.CallTo(() => fakeCartService.GetCartItemByID(A<int>._)).Returns(Task.FromResult<Cart?>(null));

            // Act
            var result = await controller.DeleteCartItem(1); 

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("CartItem with 1 does not exist", notFoundResult.Value);

            A.CallTo(() => fakeCartService.GetCartItemByID(1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeCartService.DeleteCartItem(A<int>._)).MustNotHaveHappened(); 
        }

        [Fact]
        public async Task DeleteCartItem_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            var fakeCartService = A.Fake<ICartService>();
            var fakeLogger = A.Fake<ILoggerWrapper>();
            var fakeMapper = A.Fake<IMapper>();
            var controller = new CartController(fakeCartService, fakeMapper, fakeLogger);

            A.CallTo(() => fakeCartService.GetCartItemByID(A<int>._)).Throws<Exception>();

            // Act
            var result = await controller.DeleteCartItem(1); 

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);

            A.CallTo(() => fakeCartService.GetCartItemByID(1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeCartService.DeleteCartItem(A<int>._)).MustNotHaveHappened(); 
            A.CallTo(() => fakeLogger.LogError(A<Exception>._, A<string>._)).MustHaveHappenedOnceExactly();
        }
    }
}
