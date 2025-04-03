
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
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            return Ok(employees);
        }
        [HttpGet("getbyid")]
        [Authorize]
        public async Task<IActionResult> GetById(int Id)
        {
            var employees = await _employeeService.GetEmployeeByIdAsync(Id);
            return Ok(employees);
        }
       

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] EmployeeDTO employeeDto)
        {
            var employee = await _employeeService.UpdateEmployeeAsync(employeeDto);
            return Ok(employee);
        }
        [HttpDelete("delete")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Delete(int Id)
        {
            var value = await _employeeService.DeleteEmployeeAsync(Id);
            return Ok(value);
        }
        [HttpPost("create")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Create([FromBody] EmployeeDTO employee)
        {
            var employeeEntity = await _employeeService.AddEmployeeByAsync(employee);
            return CreatedAtAction(nameof(Create), new { id = employee.Id }, employeeEntity);
        }
        [HttpGet("employeebymanager")]
        public async Task<IActionResult> GetEmployeeByManager(int id)
        {
            var employees = await _employeeService.GetAllEmployeesByManagerId(id);
            return Ok(employees);
        }
        [HttpGet("employeebydepartment")]
        public async Task<IActionResult> GetEmployeeByDepartment(int id)
        {
            var employees = await _employeeService.GetEmployeeByDepartmentId(id);
            return Ok(employees);
        }
    }
}
