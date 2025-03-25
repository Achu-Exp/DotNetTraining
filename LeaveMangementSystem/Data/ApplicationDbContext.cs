using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LeaveMangementSystem.Models;

namespace LeaveMangementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.Employee)  // A LeaveRequest has one Employee
                .WithMany(u => u.LeaveRequests)  // A User has many LeaveRequests
                .HasForeignKey(lr => lr.EmployeeId)  // Foreign key property
                .OnDelete(DeleteBehavior.Cascade); // If user is deleted, delete their leave requests
        }
    }
    }
