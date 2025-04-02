namespace LeaveManagement.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
