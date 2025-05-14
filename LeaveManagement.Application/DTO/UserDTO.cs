using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Application.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int DepartmentId { get; set; }
        public UserRole Role { get; set; }
        public int? ManagerId { get; set; }
    }
}
