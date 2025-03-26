using Microsoft.AspNetCore.Mvc;
using LeaveMangementSystem.Models.DTO;
using LeaveMangementSystem.Services;

namespace LeaveMangementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("registeruser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegRequestDTO regRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data provided.");
            }
            try
            {
                var result = _authService.RegisterUser(regRequestDTO);
                if (result == null)
                {
                    return BadRequest("Registration failed. User might already exist.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequestDTO loginRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data provided");
            }
            try
            {
                var result = _authService.LoginUser(loginRequestDTO);
                if (result == null)
                {
                    return BadRequest("User doesn't exist");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
