namespace LeaveManagement.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public int UserId { get; set; }  
        public User User { get; set; }

        public int? ManagerId { get; set; }  
        public Manager? Manager { get; set; }

        public ICollection<LeaveRequest> LeaveRequests { get; set; }
    }
}
