using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeaveMangementSystem.Models;
using LeaveMangementSystem.Services;
using LeaveMangementSystem.Models.DTO;
using LeaveMangementSystem.Services.Interfaces;

namespace LeaveMangementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveController : ControllerBase
    {
        private readonly LeaveService _leaveService;
        private readonly IEmailService _emailService;
        public LeaveController(LeaveService leaveService, IEmailService emailService)
        {
            _leaveService = leaveService;
            _emailService = emailService;
        }

        [HttpGet("getAllLeaves")]
        public async Task<IActionResult> GetLeaves()
        {
            var leaves = await _leaveService.GetAllLeaveRequests();
            return Ok(leaves);
        }

        [HttpPost("applyleave")]
        [Authorize(Roles = "Employee, Admin")]
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveRequestDTO leaveRequestDto)
        {
            await _leaveService.ApplyForLeave(leaveRequestDto);
            await _emailService.SendEmail("shalumurali2000@gmail.com", "Leave request", $"Employee {leaveRequestDto.EmployeeId} applied for leave from {leaveRequestDto.StartDate} to {leaveRequestDto.EndDate}.");
            return Ok("Applied for leave successfully, email notification send");
        }

        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ApproveLeave(int id)
        {
            await _leaveService.ApproveLeave(id);
            return Ok("Leave approved");
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
