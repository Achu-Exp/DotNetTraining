using LeaveManagement.Application.Services;
using LeaveManagement.Application.Services.Interfaces;
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
        public async Task<IActionResult> GetAll()
        {
            var leaveRequests = await _leaveRequestService.GetLeaveRequestsAsync();
            return Ok(leaveRequests);
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int Id)
        {
            var leaveRequests = await _leaveRequestService.GetLeaveRequestByIdAsync(Id);
            return Ok(leaveRequests);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] LeaveRequestDTO leaveRequest)
        {
            var Entity = await _leaveRequestService.AddLeaveRequestByAsync(leaveRequest);
            return CreatedAtAction(nameof(Create), new { id = leaveRequest.Id }, Entity);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] LeaveRequestDTO leaveRequest)
        {
            var Entity = await _leaveRequestService.UpdateLeaveRequestAsync(leaveRequest);
            return Ok(Entity);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            var value = await _leaveRequestService.DeleteLeaveRequestAsync(Id);
            return Ok(value);
        }
    }
}
