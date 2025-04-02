using LeaveManagement.Application.Services;
using LeaveManagement.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerSevice _managerService;

        public ManagerController(IManagerSevice managerService)
        {
            _managerService = managerService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var managers = await _managerService.GetManagersAsync();
            return Ok(managers);
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int Id)
        {
            var managers = await _managerService.GetManagerByIdAsync(Id);
            return Ok(managers);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ManagerDTO manager)
        {
            var managerEntity = await _managerService.AddManagerByAsync(manager);
            return CreatedAtAction(nameof(Create), new { id = manager.Id }, managerEntity);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ManagerDTO manager)
        {
            var managerEntity = await _managerService.UpdateManagerAsync(manager);
            return Ok(managerEntity);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            var value = await _managerService.DeleteManagerAsync(Id);
            return Ok(value);
        }
    }
}
