using LeaveManagement.Application.Services;
using LeaveManagement.Application.Services.Interfaces;
using LeaveManagement.Domain.Entities;
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
        public async Task<IActionResult> GetAll([FromQuery] DateOnly? startDate, [FromQuery] DateOnly? endDate,
            [FromQuery] LeaveStatus? status, [FromQuery] int? employeeId, [FromQuery] int? approverId,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            try
            {
                var leaveRequests = await _leaveRequestService.GetLeaveRequestsAsync(startDate, endDate,
                    status, employeeId, approverId, sortBy, isAscending ?? true, pageNumber, pageSize);
                if (leaveRequests == null || !leaveRequests.Any())
                {
                    return NoContent();
                }
                return Ok(leaveRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getbyid")]
        [Authorize]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var leaveRequests = await _leaveRequestService.GetLeaveRequestByIdAsync(Id);
                if (leaveRequests == null)
                {
                    return NotFound($"Leave request with ID {Id} not found");
                }
                return Ok(leaveRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("applyLeave")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Create([FromBody] LeaveRequestDTO leaveRequest)
        {
            try
            {
                if (leaveRequest == null)
                {
                    return BadRequest("Leave request data is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var Entity = await _leaveRequestService.AddLeaveRequestByAsync(leaveRequest);
                return CreatedAtAction(nameof(Create), new { id = leaveRequest.Id }, Entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
            try
            {
                var result = await _leaveRequestService.ApproveLeaveAsync(id);
                if (result == null || !result)
                {
                    return NotFound($"Leave request with ID {id} not found or could not be approved");
                }
                return Ok("Leave approved");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RejectLeave(int id)
        {
            try
            {
                var result = await _leaveRequestService.RejectLeaveAsync(id);
                if (result == null || !result)
                {
                    return NotFound($"Leave request with ID {id} not found or could not be rejected");
                }
                return Ok("Leave rejected");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var value = await _leaveRequestService.DeleteLeaveRequestAsync(Id);
                if (value <= 0)
                {
                    return NotFound($"Leave request with ID {Id} not found or could not be deleted");
                }
                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("leavesbyapprover")]
        public async Task<IActionResult> GetAllLeavesByApprover(int id)
        {
            try
            {
                var leaveRequests = await _leaveRequestService.GetLeaveRequestByApprover(id);
                if (leaveRequests == null || !leaveRequests.Any())
                {
                    return NoContent();
                }
                return Ok(leaveRequests); // Fixed return to include leaveRequests
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
