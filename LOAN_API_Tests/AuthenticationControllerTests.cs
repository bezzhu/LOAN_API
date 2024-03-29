using LOAN_API.Controllers;
using LOAN_API.Data;
using LOAN_API.Models;
using LOAN_API.Models.DTO;
using LOAN_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
namespace LOAN_API_Tests
{
    public class AuthenticationControllerTests
    {
        [Fact]
        public async Task RegisterUserWithValidUserReturnsOkAsync()
        {
            // Arrange
            var userDto = new UserDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Age = 30,
                Income = 50000.0,
                Email = "johndoe@gmail.com",
                Password = "password123"
            };

            var authenticationServiceMock = new Mock<IAuthenticationService>();
            var loggerMock = new Mock<ILogger<AuthenticationController>>();
            var userController = new AuthenticationController(authenticationServiceMock.Object, loggerMock.Object);

            // Act
            var result = await userController.Register(userDto);

            // Assert
            Assert.IsType<OkObjectResult>(result); 
            var okResult = result as OkObjectResult;
            Assert.Equal("User register successfully.", okResult.Value); 
            authenticationServiceMock.Verify(service => service.RegisterAsync(userDto), Times.Once);

        }
        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                UserName = "validUsername",
                Password = "validPassword"
            };

            var expectedToken = "validToken"; 

            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(service => service.LoginAsync(loginDto))
                                     .ReturnsAsync(expectedToken);

            var loggerMock = new Mock<ILogger<AuthenticationController>>();
            var authenticationController = new AuthenticationController(authenticationServiceMock.Object, loggerMock.Object);

            // Act
            var result = await authenticationController.Login(loginDto);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedToken, okObjectResult.Value);
        }
        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                UserName = "invalidUsername",
                Password = "invalidPassword"
            };

            var expectedErrorMessage = "Incorrect UserName or Password";

            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(service => service.LoginAsync(loginDto))
                                     .ThrowsAsync(new Exception("Invalid credentials"));

            var loggerMock = new Mock<ILogger<AuthenticationController>>();
            var authenticationController = new AuthenticationController(authenticationServiceMock.Object, loggerMock.Object);

            // Act
            var result = await authenticationController.Login(loginDto);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(expectedErrorMessage, badRequestObjectResult.Value);
        }
    }
}

