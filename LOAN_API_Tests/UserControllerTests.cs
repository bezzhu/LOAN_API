using LOAN_API.Controllers;
using LOAN_API.Models;
using LOAN_API.Models.DTO;
using LOAN_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace LOAN_API_Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userService;
        private readonly Mock<ILogger<UserController>> _logger;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _userService = new Mock<IUserService>();
            _logger = new Mock<ILogger<UserController>>();
            _userController = new UserController(_userService.Object, _logger.Object);
        }      

        [Fact]
        public async Task AddLoan_Returns_OkResult_When_Valid_LoanDto()
        {
            // Arrange
            var loanDto = new LoanDto
            {
                LoanType = LoanType.QuickLoan,
                Amount = 1000,
                Currency = "USD",
                Period = 12,
             };

            _userService.Setup(s => s.AddLoanAsync(loanDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _userController.AddLoan(loanDto);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.Equal("Loan added successfully.", okResult.Value);
            _userService.Verify(s => s.AddLoanAsync(loanDto), Times.Once);
        }
        [Fact]
        public async Task UpdateLoan_Returns_OkResult_When_Valid_LoanDto()
        {
            // Arrange
            var loanId = 123; 
            var loanDto = new LoanDto
            {
                LoanType = LoanType.QuickLoan,
                Amount = 1000,
                Currency = "USD",
                Period = 12,
            };

            _userService.Setup(s => s.UpdateLoanAsync(loanId, loanDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _userController.UpdateLoan(loanId, loanDto);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.Equal("Loan update successfully.", okResult.Value);
            _userService.Verify(s => s.UpdateLoanAsync(loanId, loanDto), Times.Once);
        }
        [Fact]
        public async Task DeleteLoan_Returns_OkResult_When_Deletion_Successful()
        {
            // Arrange
            var loanId = 123; 
            _userService.Setup(s => s.DeleteLoanAsync(loanId)).Returns(Task.CompletedTask);

            // Act
            var result = await _userController.DeleteLoan(loanId);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.Equal("Loan Delete successfully.", okResult.Value);
            _userService.Verify(s => s.DeleteLoanAsync(loanId), Times.Once);
        }

    }
}
