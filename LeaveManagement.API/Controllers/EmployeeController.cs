
using LeaveManagement.Application.Services;
using LeaveManagement.Application.Services.Interfaces;
using LeaveManagement.Domain.Entities;
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
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            return Ok(employees);
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int Id)
        {
            var employees = await _employeeService.GetEmployeeByIdAsync(Id);
            return Ok(employees);
        }
       

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] EmployeeDTO employeeDto)
        {
            var employee = await _employeeService.UpdateEmployeeAsync(employeeDto);
            return Ok(employee);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            var value = await _employeeService.DeleteEmployeeAsync(Id);
            return Ok(value);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] EmployeeDTO employee)
        {
            var employeeEntity = await _employeeService.AddEmployeeByAsync(employee);
            return CreatedAtAction(nameof(Create), new { id = employee.Id }, employeeEntity);
        }
    }
}
