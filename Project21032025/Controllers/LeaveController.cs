using Microsoft.AspNetCore.Mvc;
using Project21032025.Models;
using Project21032025.Services;

namespace Project21032025.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveController : ControllerBase
    {
        private readonly LeaveService _leaveService;
        public LeaveController(LeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpGet("getAllLeaves")]
        public async Task<IActionResult> GetLeaves()
        {
            var leaves = await _leaveService.GetAllLeaveRequests();
            return Ok(leaves);
        }

        [HttpPost("addLeave")]
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveRequest leaveRequest)
        {
            await _leaveService.ApplyForLeave(leaveRequest);
            return Ok("Applied for leave successfully");
        }
    }
}
