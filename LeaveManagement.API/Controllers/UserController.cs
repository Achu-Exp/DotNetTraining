using LeaveManagement.Application.Services;
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
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int Id)
        {
            var users = await _userService.GetUserByIdAsync(Id);
            return Ok(users);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserDTO user)
        {
            var userEntity = await _userService.AddUserByAsync(user);
            return CreatedAtAction(nameof(Create), new { id = user.Id }, userEntity);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UserDTO user)
        {
            var userEntity = await _userService.UpdateUserAsync(user);
            return Ok(userEntity);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            var value = await _userService.DeleteUserAsync(Id);
            return Ok(value);
        }
    }
}
