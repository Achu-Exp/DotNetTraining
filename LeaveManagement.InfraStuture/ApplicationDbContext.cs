using LeaveManagement.Infrastructure.Configuration;

namespace LeaveManagement.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }  
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Manager> Managers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfig());
            modelBuilder.ApplyConfiguration(new LeaveRequestConfig());
            modelBuilder.ApplyConfiguration(new ManagerConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());  
            modelBuilder.ApplyConfiguration(new DepartmentConfig());
        }
    }
}
