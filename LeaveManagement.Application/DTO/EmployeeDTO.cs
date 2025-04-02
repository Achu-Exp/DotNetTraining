using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.Application.DTO
{
    public record EmployeeDTO
    {
        public int Id { get; set; }
        public UserDTO User { get; set; }
        public int? ManagerId { get; set; }
    }
}
