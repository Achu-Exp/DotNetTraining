using LeaveManagement.Application.Services;
using LeaveManagement.Application.Services.Interfaces;
using LeaveManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController: ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("getall")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 1000)
        {
            try
            {
                var employees = await _employeeService.GetEmployeesAsync(filterOn, filterQuery,
                    sortBy, isAscending?? true, pageNumber, pageSize);
                if (employees == null || !employees.Any())
                {
                    return NoContent();
                }
                return Ok(employees);
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
                var employees = await _employeeService.GetEmployeeByIdAsync(Id);
                if (employees == null)
                {
                    return NotFound($"Employee with ID {Id} not found");
                }
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] EmployeeDTO employeeDto)
        {
            try
            {
                if (employeeDto == null)
                {
                    return BadRequest("Employee data is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var employee = await _employeeService.UpdateEmployeeAsync(employeeDto);
                if (employee == null)
                {
                    return NotFound($"Employee with ID {employeeDto.Id} not found");
                }
                return Ok(employee);
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
                var value = await _employeeService.DeleteEmployeeAsync(Id);
                if (value <= 0)
                {
                    return NotFound($"Employee with ID {Id} not found or could not be deleted");
                }
                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Create([FromBody] EmployeeDTO employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest("Employee data is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var employeeEntity = await _employeeService.AddEmployeeByAsync(employee);
                return CreatedAtAction(nameof(Create), new { id = employee.Id }, employeeEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("managerId")]
        public async Task<IActionResult> GetEmployeeByManager(int id)
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesByManagerId(id);
                if (employees == null || !employees.Any())
                {
                    return Ok(employees);
                }
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("departmentId")]
        public async Task<IActionResult> GetEmployeeByDepartment(int id)
        {
            try
            {
                var employees = await _employeeService.GetEmployeeByDepartmentId(id);
                if (employees == null || !employees.Any())
                {
                    return Ok(employees);
                }
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
