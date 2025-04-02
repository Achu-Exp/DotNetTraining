namespace LeaveManagement.Infrastructure.Configuration
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");

            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(d => d.Description)
                .HasMaxLength(255);
        }
    }
}
