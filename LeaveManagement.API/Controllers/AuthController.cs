using LeaveManagement.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;


        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> LoginUser([FromBody] LoginRequestDTO request)
        {
            var response = await _authService.LoginUser(request);
            return Ok(response);
        }
    }
}
