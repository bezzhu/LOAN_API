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
    public class AccoutantControllerTests
    {
        private readonly Mock<IAccoutantService> _accountantService;
        private readonly Mock<ILogger<AccountantController>> _logger;
        private readonly AccountantController _accountantController;

        public AccoutantControllerTests()
        {
            _accountantService = new Mock<IAccoutantService>();
            _logger = new Mock<ILogger<AccountantController>>();
            _accountantController = new AccountantController(_accountantService.Object, _logger.Object);
        }
        [Fact]
        public async Task GetLoans_Returns_OkResult_When_Retrieval_Successful()
        {
            // Arrange
            var userId = 123;

            _accountantService.Setup(s => s.GetLoansAsync(userId));

            // Act
            var result = await _accountantController.GetLoans(userId);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            _accountantService.Verify(s => s.GetLoansAsync(userId), Times.Once);
        }
        [Fact]
        public async Task UpdateLoan_Returns_OkResult_When_Update_Successful()
        {
            // Arrange
            var loanId = 123; 
            var updatedLoanDto = new LoanDto
            {
                LoanType = LoanType.QuickLoan,
                Amount = 1500,
                Currency = "USD",
                Period = 24,
            };

            _accountantService.Setup(s => s.UpdateLoanAsync(loanId, updatedLoanDto));

            // Act
            var result = await _accountantController.UpdateLoan(loanId, updatedLoanDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal("Loan update successfully.", okResult.Value);
            _accountantService.Verify(s => s.UpdateLoanAsync(loanId, updatedLoanDto), Times.Once);
        }
        [Fact]
        public async Task BlockUser_Returns_OkResult_When_Block_Successful()
        {
            // Arrange
            var userId = 123; //

            _accountantService.Setup(s => s.BlockUserAsync(userId)).Returns(Task.CompletedTask);

            // Act
            var result = await _accountantController.BlockUser(userId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal("User block successfully.", okResult.Value);
            _accountantService.Verify(s => s.BlockUserAsync(userId), Times.Once);
        }
        [Fact]
        public async Task DeleteLoan_Returns_OkResult_When_Deletion_Successful()
        {
            // Arrange
            var loanId = 123; 

            _accountantService.Setup(s => s.DeleteLoanAsync(loanId)).Returns(Task.CompletedTask);

            // Act
            var result = await _accountantController.DeleteLoan(loanId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal("Loan delete successfully.", okResult.Value);
            _accountantService.Verify(s => s.DeleteLoanAsync(loanId), Times.Once);
        }


    }
}
