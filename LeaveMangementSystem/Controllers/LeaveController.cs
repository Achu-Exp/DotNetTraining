using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeaveMangementSystem.Models;
using LeaveMangementSystem.Services;
using LeaveMangementSystem.Models.DTO;

namespace LeaveMangementSystem.Controllers
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
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveRequestDTO leaveRequestDto)
        {
            await _leaveService.ApplyForLeave(leaveRequestDto);
            return Ok("Applied for leave successfully");
        }

        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ApproveLeave(int id)
        {
            await _leaveService.ApproveLeave(id);
            return Ok("Leave approed");
        }

        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RejectLeave(int id)
        {
            await _leaveService.RejectLeave(id);
            return Ok("Leave rejected");
        }
    }
}
