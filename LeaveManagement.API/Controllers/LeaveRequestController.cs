using LeaveManagement.Application.Services;
using LeaveManagement.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpGet("getall")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var leaveRequests = await _leaveRequestService.GetLeaveRequestsAsync();
            return Ok(leaveRequests);
        }
        [HttpGet("getbyid")]
        [Authorize]
        public async Task<IActionResult> GetById(int Id)
        {
            var leaveRequests = await _leaveRequestService.GetLeaveRequestByIdAsync(Id);
            return Ok(leaveRequests);
        }
        [HttpPost("applyLeave")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Create([FromBody] LeaveRequestDTO leaveRequest)
        {
            var Entity = await _leaveRequestService.AddLeaveRequestByAsync(leaveRequest);
            return CreatedAtAction(nameof(Create), new { id = leaveRequest.Id }, Entity);
        }

        //[HttpPut("update")]
        //public async Task<IActionResult> Update([FromBody] LeaveRequestDTO leaveRequest)
        //{
        //    var Entity = await _leaveRequestService.UpdateLeaveRequestAsync(leaveRequest);
        //    return Ok(Entity);
        //}

        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ApproveLeave(int id)
        {
            await _leaveRequestService.ApproveLeaveAsync(id);
            return Ok("Leave approved");
        }

        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RejectLeave(int id)
        {
            await _leaveRequestService.RejectLeaveAsync(id);
            return Ok("Leave rejected");
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Delete(int Id)
        {
            var value = await _leaveRequestService.DeleteLeaveRequestAsync(Id);
            return Ok(value);
        }
    }
}
