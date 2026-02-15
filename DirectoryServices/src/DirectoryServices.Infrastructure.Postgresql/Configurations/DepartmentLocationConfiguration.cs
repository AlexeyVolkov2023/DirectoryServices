using DirectoryServices.Domain.DepartmentManagement.Supporting;
using DirectoryServices.Domain.LocationManagement.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryServices.Infrastructure.Configurations;

public class DepartmentLocationConfiguration : IEntityTypeConfiguration<DepartmentLocation>
{
    public void Configure(EntityTypeBuilder<DepartmentLocation> builder)
    {
        builder.ToTable("department_locations");

        builder.HasKey(dp => dp.DepartmentLocationId).HasName("pk_department_location");

        builder.Property(d => d.DepartmentLocationId)
            .HasConversion(id => id.Value, value => DepartmentLocationId.Create(value))
            .HasColumnName("id");

        builder
            .HasOne(dp => dp.Department)
            .WithMany(d => d.DepartmentLocations)
            .HasForeignKey("department_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Location>()
            .WithMany()
            .HasForeignKey(dp => dp.LocationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(dl => dl.LocationId)
            .HasColumnName("location_id")
            .IsRequired();

        builder.Property(dp => dp.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
    }
}