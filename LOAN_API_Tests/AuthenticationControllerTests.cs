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
        private readonly Mock<IAuthenticationService> _authenticationService;
        private readonly Mock<ILogger<AuthenticationController>> _logger;
        private readonly AuthenticationController _authenticationController;

        public AuthenticationControllerTests()
        {
            _authenticationService = new Mock<IAuthenticationService>();
            _logger = new Mock<ILogger<AuthenticationController>>();
            _authenticationController = new AuthenticationController(_authenticationService.Object, _logger.Object);

        }
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
            };;

            // Act
            var result = await _authenticationController.Register(userDto);

            // Assert
            Assert.IsType<OkObjectResult>(result); 
            var okResult = result as OkObjectResult;
            Assert.Equal("User register successfully.", okResult.Value);
            _authenticationService.Verify(service => service.RegisterAsync(userDto), Times.Once);

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

            _authenticationService.Setup(service => service.LoginAsync(loginDto))
                                     .ReturnsAsync(expectedToken);

            // Act
            var result = await _authenticationController.Login(loginDto);

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

            _authenticationService.Setup(service => service.LoginAsync(loginDto))
                                     .ThrowsAsync(new Exception("Incorrect UserName or Password"));

            // Act
            var result = await _authenticationController.Login(loginDto);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(expectedErrorMessage, badRequestObjectResult.Value);
        }
    }
}

