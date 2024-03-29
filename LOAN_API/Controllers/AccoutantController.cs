using LOAN_API.Models.DTO;
using LOAN_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LOAN_API.Services;
using System;
using System.Linq;
using LOAN_API.Validators;
using Microsoft.Extensions.Logging;

namespace LOAN_API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AccoutantController : Controller
    {
        private readonly IAccoutantService _accoutantService;
        private readonly ILogger<AccoutantController> _logger;
        public AccoutantController(IAccoutantService accoutantService
                                            ,ILogger<AccoutantController> logger)
        {
            _accoutantService = accoutantService;
            _logger = logger;


        }

        [HttpGet("getAllLoans/{userId}")]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans(int userId)
        {
            try
            {
                var loans = await _accoutantService.GetLoansAsync(userId);

                return Ok(loans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest($"Failed to getAlLoans: {ex.Message}");
            }

        }

        [HttpPut("updateLoan/{loanId}")]
        public async Task<ActionResult> UpdateLoan(int loanId, [FromBody] LoanDto updatedLoanDto)
        {
            try
            {
                var validator = new LoanDtoValidator();
                var result = validator.Validate(updatedLoanDto);

                if (!result.IsValid)
                {
                    var errors = result.Errors.Select(error => error.ErrorMessage).ToList();
                    return BadRequest(errors);
                }

                await _accoutantService.UpdateLoanAsync(loanId, updatedLoanDto);

                return Ok("Loan update successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest($"Failed to update loan: {ex.Message}");
            }
        }

        [HttpPut("blockUser/{userId}")]
        public async Task<ActionResult> BlockUser(int userId)
        {
            try
            {
                await _accoutantService.BlockUserAsync(userId);
                return Ok("User block successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest($"Failed to block user: {ex.Message}");
            }

        }

        [HttpDelete("deleteLoan/{loanId}")]
        public async Task<IActionResult> DeleteLoan(int loanId)
        {
            try
            {
                await _accoutantService.DeleteLoanAsync(loanId);

                return Ok("Loan delete successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest($"Failed to delete loan: {ex.Message}");
            }
        }

        

    }
}
