using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Controllers;
using ShoppingCart.DTOs;
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
    public class CartItemsTotalPriceTotalTest
    {

        [Fact]
        public async Task GetTotalItemsAndTotalPrice_ReturnsOk_WhenServiceSucceeds()
        {
            // Arrange
            var fakeCartService = A.Fake<ICartService>(); 
            var fakeMapper = A.Fake<IMapper>();
            var fakeLogger=A.Fake<ILoggerWrapper>();

            var controller = new CartController(fakeCartService, fakeMapper, fakeLogger);
            var fakeCartAndPriceItems = new CartAndPriceItemTotal();


            A.CallTo(() => fakeCartService.GetTotalItemsAndTotalPrice()).Returns(Task.FromResult(fakeCartAndPriceItems));

            var expectedDto = new GetCartAndPriceItemsDTO();
            A.CallTo(() => fakeMapper.Map<GetCartAndPriceItemsDTO>(fakeCartAndPriceItems)).Returns(expectedDto);

            // Act
            var result = await controller.GetTotalItemsAndTotalPrice();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var dtoResult = Assert.IsType<GetCartAndPriceItemsDTO>(okResult.Value);
            Assert.Same(expectedDto, dtoResult);

            A.CallTo(() => fakeCartService.GetTotalItemsAndTotalPrice()).MustHaveHappenedOnceExactly(); 
            A.CallTo(() => fakeMapper.Map<GetCartAndPriceItemsDTO>(fakeCartAndPriceItems)).MustHaveHappenedOnceExactly(); 
        }

        [Fact]
        public async Task GetTotalItemsAndTotalPrice_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var fakeCartService = A.Fake<ICartService>();
            var fakeMapper = A.Fake<IMapper>();
            var fakeLogger = A.Fake<ILoggerWrapper>();

            var controller = new CartController(fakeCartService, fakeMapper, fakeLogger);

            A.CallTo(() => fakeCartService.GetTotalItemsAndTotalPrice()).Throws<Exception>();

            // Act
            var result = await controller.GetTotalItemsAndTotalPrice();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);

            A.CallTo(() => fakeCartService.GetTotalItemsAndTotalPrice()).MustHaveHappenedOnceExactly(); 
            A.CallTo(() => fakeLogger.LogError(A<Exception>._, A<string>._)).MustHaveHappenedOnceExactly(); 
        }

    }
}
