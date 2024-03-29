using LOAN_API.Models;
using LOAN_API.Models.DTO;
using LOAN_API.Services;
using LOAN_API.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LOAN_API.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService  _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
             _userService = userService;
                _logger = logger;
        }      

        [HttpGet("getInfo")]
        public async Task<ActionResult<User>> GetInfo()
        {
            var currentUserId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
           
            var user = await  _userService.GetInfoAsync(currentUserId);

            return user;
        }
       
        [HttpPost("addLoan")]
        public async Task<ActionResult<Loan>> AddLoan([FromBody] LoanDto loanDto)
        {
            try
            {
                var validator = new LoanDtoValidator();
                var result = validator.Validate(loanDto);

                if (!result.IsValid)
                {
                    var errors = result.Errors.Select(error => error.ErrorMessage).ToList();
                    return BadRequest(errors);
                }

                await  _userService.AddLoanAsync(loanDto);

                return Ok("Loan added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("updateLoan/{loanId}")]
        public async Task<ActionResult<Loan>> UpdateLoan(int loanId , [FromBody] LoanDto loanDto)
        {
            try
            {
                var validator = new LoanDtoValidator();
                var result = validator.Validate(loanDto);

                if (!result.IsValid)
                {
                    var errors = result.Errors.Select(error => error.ErrorMessage).ToList();
                    return BadRequest(errors);
                }

                await  _userService.UpdateLoanAsync(loanId, loanDto);

                return Ok("Loan update successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("deleteLoan/{loanId}")]
        public async Task<ActionResult<Loan>> DeleteLoan(int loanId)
        {
            try
            {
                await  _userService.DeleteLoanAsync(loanId);

                return Ok("Loan Delete successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

        }
        

    }
}
