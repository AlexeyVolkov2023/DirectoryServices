using DirectoryServices.Domain.DepartmentManagement.Supporting;
using DirectoryServices.Domain.PositionManagement.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryServices.Infrastructure.Configurations;

public class DepartmentPositionConfiguration : IEntityTypeConfiguration<DepartmentPosition>
{
    public void Configure(EntityTypeBuilder<DepartmentPosition> builder)
    {
        builder.ToTable("department_positions");

        builder.HasKey(dp => dp.DepartmentPositionId).HasName("pk_department_position");

        builder.Property(d => d.DepartmentPositionId)
            .HasConversion(id => id.Value, value => DepartmentPositionId.Create(value))
            .HasColumnName("id");

        builder
            .HasOne(dp => dp.Department)
            .WithMany(d => d.DepartmentPositions)
            .HasForeignKey("department_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Position>()
            .WithMany()
            .HasForeignKey(dp => dp.PositionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(dl => dl.PositionId)
            .HasColumnName("position_id")
            .IsRequired();

        builder.Property(dp => dp.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
    }
}