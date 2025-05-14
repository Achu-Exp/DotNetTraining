namespace LeaveManagement.Application.DTO
{
    public record EmployeeDTO
    {
        public int Id { get; set; }
        public UsersDTO User { get; set; }
        public int? ManagerId { get; set; }
    }
}
