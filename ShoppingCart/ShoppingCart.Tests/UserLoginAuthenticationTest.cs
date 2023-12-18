using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using ShoppingCart.Controllers;
using ShoppingCart.LoggingError;
using ShoppingCart.Models;
using ShoppingCart.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace ShoppingCart.Tests
{
 public class LoginTest
   {

        [Fact]
        public async void Login_ValidUser_ReturnsOkObjectResultWithToken()
        {
            // Arrange
            var cartUserService = A.Fake<ICartUserService>();
           
            var logger = A.Fake<ILoggerWrapperLogin>();
            var configuration = A.Fake<IConfiguration>();

            var controller = new CartUserLoginController(cartUserService, logger,configuration);
           
            var validLogin = new CartUserLogin
            {
               UserName="Mark",
               Password="Mark"
            };
            var user = new CartUsersModel
            {
                Email = "",
                UserName = "",
                GivenName = "",
                Role = ""
            };
            var fakeuser=A.Fake<CartUsersModel>();
            fakeuser.UserName = user.UserName;
            fakeuser.Email = user.Email;
            fakeuser.GivenName=user.GivenName;
            fakeuser.Role=user.Role;

            A.CallTo(() => configuration["Jwt:Key"]).Returns("KDHsofmIwydpLJSIbsUBDNososPwiqbUwy2uwu5sdiswOs");

            TokenGeneratorService generateservice = new TokenGeneratorService(logger,configuration);
            var token = generateservice.Generate(fakeuser);

           
            A.CallTo(() => cartUserService.UserLogin(validLogin)).Returns<CartUsersModel>(user);
     

            // Act
            var result =await controller.Login(validLogin);

            // Assert 
            Assert.IsType<OkObjectResult>(result);
            
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
          
            Assert.IsType<string>(okResult.Value);

        }


        [Fact]
        public async void Login_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var fakeCartUserService = A.Fake<ICartUserService>();
            var fakeLogger = A.Fake<ILoggerWrapperLogin>();
            var fakeConfiguration = A.Fake<IConfiguration>();
            var controller = new CartUserLoginController(fakeCartUserService,fakeLogger,fakeConfiguration);

            var loginRequest = new CartUserLogin
            {
                // Set properties or initialize the object as needed for testing
                UserName = "NonExistentUser",
                Password = "TestPassword"
            };

            A.CallTo(() => fakeCartUserService.UserLogin(loginRequest)).Returns(Task.FromResult<CartUsersModel>(null));

            // Act
            var result = await controller.Login(loginRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("Cart User cannot be found", notFoundResult.Value);

            A.CallTo(() => fakeCartUserService.UserLogin(loginRequest)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void Login_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            var fakeCartUserService = A.Fake<ICartUserService>();
            var fakeLogger = A.Fake<ILoggerWrapperLogin>();
            var fakeConfiguration = A.Fake<IConfiguration>();
            var controller = new CartUserLoginController(fakeCartUserService,fakeLogger,fakeConfiguration);

            var loginRequest = new CartUserLogin
            {
                // Set properties or initialize the object as needed for testing
                UserName = "TestUser",
                Password = "TestPassword"
            };

            A.CallTo(() => fakeCartUserService.UserLogin(loginRequest)).Throws<Exception>();

            // Act
            var result = await controller.Login(loginRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);

            A.CallTo(() => fakeCartUserService.UserLogin(loginRequest)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeLogger.LogError(A<Exception>._, A<string>._)).MustHaveHappenedOnceExactly();
        }
    }
}
