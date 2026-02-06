using ExpenseTracker.Data;
using ExpenseTracker.Dtos;
using ExpenseTracker.Repositories;
using ExpenseTracker.Repositories.interfaces;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFileLoggerService _logger;

        public UserController(IUserService userService, IFileLoggerService logger)
        {
            _userService = userService;
            _logger = logger;
        }

        #region Registration

        [HttpPost("Register")]
        public async Task<ActionResult<object>> Register([FromBody] UserRegisterDto userDto)
        {
            try
            {
                await _logger.LogInformationAsync("User registration attempt", new { Email = userDto.Email });
                
                var user = await _userService.RegisterUserAsync(userDto);
                
                await _logger.LogInformationAsync("User registered successfully", new { UserId = user.Id });
                
                return CreatedAtAction(nameof(Register), new { id = user.Id }, new { message = "User registered successfully", userId = user.Id });
            }
            catch (InvalidOperationException ex)
            {
                await _logger.LogWarningAsync("User registration failed", new { Email = userDto.Email, Error = ex.Message });
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Error during user registration", ex, new { Email = userDto.Email });
                return StatusCode(500, new { message = "An error occurred during registration" });
            }
        }

        #endregion

        #region Login

        [HttpPost("Login")]
        public async Task<ActionResult<object>> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                await _logger.LogInformationAsync("User login attempt", new { Email = userLoginDto.Email });
                
                var token = await _userService.LoginUserAsync(userLoginDto);
                
                await _logger.LogInformationAsync("User login successful", new { Email = userLoginDto.Email });
                
                return Ok(new { message = "Login successful", token });
            }
            catch (UnauthorizedAccessException ex)
            {
                await _logger.LogWarningAsync("User login failed", new { Email = userLoginDto.Email, Error = ex.Message });
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Error during user login", ex, new { Email = userLoginDto.Email });
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }

        #endregion
    }
}