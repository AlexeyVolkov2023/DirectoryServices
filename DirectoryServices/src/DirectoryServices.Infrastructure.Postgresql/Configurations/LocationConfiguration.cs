using DirectoryServices.Domain;
using DirectoryServices.Domain.LocationManagement.Aggregate;
using DirectoryServices.Domain.LocationManagement.Id;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryServices.Infrastructure.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("location");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasConversion(id => id.Value, value => LocationId.Create(value))
            .HasColumnName("id");

        builder.ComplexProperty(l => l.LocationName, lnb =>
        {
            lnb.Property(ln => ln.Value)
                .HasColumnName("location_name")
                .HasMaxLength(LengthConstants.Length120)
                .IsRequired();
        });

        builder.ComplexProperty(l => l.Address, ab =>
        {
            ab.Property(a => a.Country)
                .HasColumnName("country")
                .IsRequired();
            ab.Property(a => a.Region)
                .HasColumnName("region")
                .IsRequired();
            ab.Property(a => a.City)
                .HasColumnName("city")
                .IsRequired();
            ab.Property(a => a.Street)
                .HasColumnName("street")
                .IsRequired();
            ab.Property(a => a.HouseNumber)
                .HasColumnName("house_number")
                .IsRequired();
        });

        builder.ComplexProperty(l => l.Timezone, tzb =>
        {
            tzb.Property(tz => tz.Value)
                .HasColumnName("timezone")
                .IsRequired();
        });

        builder.Property(l => l.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(l => l.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(l => l.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
    }
}