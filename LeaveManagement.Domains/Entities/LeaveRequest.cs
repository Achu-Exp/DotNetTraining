namespace LeaveManagement.Domain.Entities
{
    public class LeaveRequest : BaseEntity
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Reason { get; set; }
        public LeaveStatus Status { get; set; }

        // Foreign Key - Employee who made the request
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        // Foreign Key - Manager who approves the request (nullable in case it's pending)
        public int? ApproverId { get; set; }
        public Manager? Approver { get; set; }
    }
}
