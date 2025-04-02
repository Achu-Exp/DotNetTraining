namespace LeaveManagement.Infrastructure.Configuration
{
    public class ManagerConfig : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.ToTable("Managers");

            builder.HasKey(m => m.Id);

            // Manager has a foreign key to User
            builder.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Manager has multiple Employees
            builder.HasMany(m => m.Employees)
                .WithOne(e => e.Manager)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

