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
    public class UpdateCartTest
    {
        [Fact]
        public async Task UpdateCartItem_ReturnsNoContent_WhenServiceSucceeds()
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

            var updatedCart = new Cart
            {
                ProductId = 1,
                Name = "UpdatedProduct",
                Price = 15.0,
                Quantity = 8
            };

            A.CallTo(() => fakeCartService.UpdateCartItem(updatedCart)).Returns(Task.FromResult<Cart?>(updatedCart));

            // Act
            var result = await controller.UpdateCartItem(updatedCart);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Cart>>(result);
            Assert.IsType<NoContentResult>(actionResult.Result);

            A.CallTo(() => fakeCartService.GetCartItemByID(existingCart.ProductId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeCartService.UpdateCartItem(updatedCart)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeLogger.LogError(A<Exception>._, A<string>._)).MustNotHaveHappened();
        }
        [Fact]
        public async Task UpdateCartItem_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            var fakeCartService = A.Fake<ICartService>();
            var fakeLogger = A.Fake<ILoggerWrapper>();
            var fakeMapper = A.Fake<IMapper>();
            var controller = new CartController(fakeCartService, fakeMapper, fakeLogger);

            var updatedCart = new Cart
            {
                ProductId = 1,
                Name = "UpdatedProduct",
                Price = 15.0,
                Quantity = 8
            };

            A.CallTo(() => fakeCartService.GetCartItemByID(updatedCart.ProductId)).Returns(Task.FromResult<Cart?>(new Cart())); // Return a non-null value
            A.CallTo(() => fakeCartService.UpdateCartItem(updatedCart)).Throws<Exception>();

            // Act
            var result = await controller.UpdateCartItem(updatedCart);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Cart>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);

            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);

            A.CallTo(() => fakeCartService.GetCartItemByID(updatedCart.ProductId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeCartService.UpdateCartItem(updatedCart)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeLogger.LogError(A<Exception>._, A<string>._)).MustHaveHappenedOnceExactly();
        }


    }
}
