using DirectoryServices.Domain;
using DirectoryServices.Domain.DepartmentManagement.Aggregate;
using DirectoryServices.Domain.DepartmentManagement.Id;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryServices.Infrastructure.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(id => id.Value, value => DepartmentId.Create(value))
            .HasColumnName("id");

        builder.ComplexProperty(d => d.DepartmentName, dnb =>
        {
            dnb.Property(dn => dn.Value)
                .HasColumnName("department_name")
                .HasMaxLength(LengthConstants.Length150)
                .IsRequired();
        });

        builder.ComplexProperty(d => d.Identifier, ib =>
        {
            ib.Property(i => i.Value)
                .HasColumnName("identifier")
                .HasMaxLength(LengthConstants.Length150)
                .IsRequired();
        });

        builder.ComplexProperty(d => d.Path, pb =>
        {
            pb.Property(p => p.Value)
                .HasColumnName("path")
                .HasMaxLength(LengthConstants.Length1000)
                .IsRequired();
        });

        builder.ComplexProperty(d => d.Depth, db =>
        {
            db.Property(d => d.Value)
                .HasColumnName("depth")
                .IsRequired();
        });

        builder.Property(d => d.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(d => d.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(d => d.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasOne(d => d.Parent)
            .WithMany(p => p.Children)
            .HasForeignKey("parent_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}