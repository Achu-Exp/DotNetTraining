using LeaveManagement.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpGet("getall")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var departments = await _departmentService.GetDepartmentsAsync();
                if (departments == null || !departments.Any())
                {
                    return NoContent();
                }
                return Ok(departments);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getbyid")]
        [Authorize]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var departments = await _departmentService.GetDepartmentByIdAsync(Id);
                if (departments == null)
                {
                    return NotFound($"Department with ID {Id} not found");
                }
                return Ok(departments);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] DepartmentDTO department)
        {
            try
            {
                if (department == null)
                {
                    return BadRequest("Department data is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var departmentEntity = await _departmentService.AddDepartmentByAsync(department);
                return CreatedAtAction(nameof(Create), new { id = department.Id }, departmentEntity);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] DepartmentDTO department)
        {
            try
            {
                if (department == null)
                {
                    return BadRequest("Department data is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var departmentEntity = await _departmentService.UpdateDepartmentAsync(department);
                if (departmentEntity == null)
                {
                    return NotFound($"Department with ID {department.Id} not found");
                }
                return Ok(departmentEntity);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var value = await _departmentService.DeleteDepartmentAsync(Id);
                if (value <= 0)
                {
                    return NotFound($"Department with ID {Id} not found or could not be deleted");
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
