namespace LeaveManagement.Infrastructure.Configuration
{
    internal class LeaveRequestConfig : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.ToTable("LeaveRequests");

            builder.HasKey(lr => lr.Id);
            builder.Property(lr => lr.StartDate)
                .IsRequired();
            builder.Property(lr => lr.EndDate)
                .IsRequired();
            builder.Property(lr => lr.Reason)
                .HasMaxLength(500);
            builder.Property(lr => lr.Status)
                .IsRequired();

            // Relationship: LeaveRequest belongs to an Employee
            builder.HasOne(lr => lr.Employee)
                .WithMany(e => e.LeaveRequests)
                .HasForeignKey(lr => lr.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: LeaveRequest is approved by a Manager
            builder.HasOne(lr => lr.Approver)
                .WithMany()
                .HasForeignKey(lr => lr.ApproverId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
