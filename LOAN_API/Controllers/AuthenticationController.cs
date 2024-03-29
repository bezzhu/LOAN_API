using LOAN_API.Models.DTO;
using LOAN_API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using LOAN_API.Validator;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace LOAN_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService
                                        ,ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {       
            try
            {
                var validator = new UserDtoValidator();
                var result = validator.Validate(userDto);

                if(!result.IsValid)
                {
                    var errors = result.Errors.Select(error => error.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
                await _authenticationService.RegisterAsync(userDto);
                return Ok("User register successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _authenticationService.LoginAsync(loginDto);
                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
