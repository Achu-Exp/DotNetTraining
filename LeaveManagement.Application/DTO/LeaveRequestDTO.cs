using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Application.DTO
{
    public record LeaveRequestDTO
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Reason { get; set; }
        public LeaveStatus Status { get; set; }
        public int EmployeeId { get; set; }
        public int ApproverId { get; set; }
    }
}
