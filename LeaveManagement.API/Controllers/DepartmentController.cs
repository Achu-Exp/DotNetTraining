using LeaveManagement.Application.Services;
using LeaveManagement.Application.Services.Interfaces;
using LeaveManagement.Domain.Entities;
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
            var departments = await _departmentService.GetDepartmentsAsync();
            return Ok(departments);
        }
        [HttpGet("getbyid")]
        [Authorize]
        public async Task<IActionResult> GetById(int Id)
        {
            var departments = await _departmentService.GetDepartmentByIdAsync(Id);
            return Ok(departments);
        }
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] DepartmentDTO department)
        {
            var departmentEntity = await _departmentService.AddDepartmentByAsync(department);
            return CreatedAtAction(nameof(Create), new { id = department.Id }, departmentEntity);
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update([FromBody] DepartmentDTO department)
        {
            var departmentEntity = await _departmentService.UpdateDepartmentAsync(department);
            return Ok(departmentEntity);
        }
        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int Id)
        {
            var value = await _departmentService.DeleteDepartmentAsync(Id);
            return Ok(value);
        }

    }
}
