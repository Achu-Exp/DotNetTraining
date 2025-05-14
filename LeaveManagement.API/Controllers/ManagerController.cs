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
            try
            {
                var managers = await _managerService.GetManagersAsync();
                if (managers == null || !managers.Any())
                {
                    return NoContent();
                }
                return Ok(managers);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var managers = await _managerService.GetManagerByIdAsync(Id);
                if (managers == null)
                {
                    return NotFound($"Manager with ID {Id} not found");
                }
                return Ok(managers);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ManagerDTO manager)
        {
            try
            {
                if (manager == null)
                {
                    return BadRequest("Manager data is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var managerEntity = await _managerService.AddManagerByAsync(manager);
                return CreatedAtAction(nameof(Create), new { id = manager.Id }, managerEntity);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ManagerDTO manager)
        {
            try
            {
                if (manager == null)
                {
                    return BadRequest("Manager data is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var managerEntity = await _managerService.UpdateManagerAsync(manager);
                if (managerEntity == null)
                {
                    return NotFound($"Manager with ID {manager.Id} not found");
                }

                return Ok(managerEntity);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var value = await _managerService.DeleteManagerAsync(Id);

                // Assuming DeleteManagerAsync returns an int that indicates affected rows
                if (value <= 0)
                {
                    return NotFound($"Manager with ID {Id} not found or could not be deleted");
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
