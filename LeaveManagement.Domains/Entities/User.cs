namespace LeaveManagement.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
