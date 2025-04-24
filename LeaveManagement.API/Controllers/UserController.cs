using LeaveManagement.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetUsersAsync();

                if (users == null || !users.Any())
                {
                    return NoContent(); // 204 No Content is appropriate when there are no users
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return 500 status code for server errors instead of 400 BadRequest
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var users = await _userService.GetUserByIdAsync(Id);
                if (users == null)
                {
                    return NotFound($"User with ID {Id} not found");
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserDTO user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User data is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userEntity = await _userService.AddUserByAsync(user);
                return CreatedAtAction(nameof(Create), new { id = user.Id }, userEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UserDTO user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User data is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userEntity = await _userService.UpdateUserAsync(user);
                if (userEntity == null)
                {
                    return NotFound($"User with ID {user.Id} not found");
                }

                return Ok(userEntity);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var value = await _userService.DeleteUserAsync(Id);
                // Assuming DeleteUserAsync returns an int that represents:
                // - 0 or negative value if user wasn't found/couldn't be deleted
                // - 1 or positive value if deletion was successful
                if (value <= 0)
                {
                    return NotFound($"User with ID {Id} not found or could not be deleted");
                }

                return Ok(value);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
