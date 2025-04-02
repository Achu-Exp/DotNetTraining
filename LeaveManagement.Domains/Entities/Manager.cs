namespace LeaveManagement.Domain.Entities
{
    public class Manager :BaseEntity
    {
        public int UserId { get; set; }  
        public User User { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
