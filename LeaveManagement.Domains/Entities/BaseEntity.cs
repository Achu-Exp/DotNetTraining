namespace LeaveManagement.Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
    }


    public enum LeaveStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }


}
    


