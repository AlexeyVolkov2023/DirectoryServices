using DirectoryServices.Domain;
using DirectoryServices.Domain.PositionManagement.Aggregate;
using DirectoryServices.Domain.PositionManagement.Id;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryServices.Infrastructure.Configurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("position");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, value => PositionId.Create(value))
            .HasColumnName("id");

        builder.ComplexProperty(p => p.PositionName, pnb =>
        {
            pnb.Property(pn => pn.Value)
                .HasColumnName("position_name")
                .HasMaxLength(LengthConstants.Length100)
                .IsRequired();
        });

        builder.ComplexProperty(p => p.Description, db =>
        {
            db.Property(d => d.Value)
                .HasColumnName("description")
                .HasMaxLength(LengthConstants.Length1000)
                .IsRequired();
        });

        builder.Property(p => p.IsActive)
            .HasColumnName("is_active")
            .IsRequired();
    }
}