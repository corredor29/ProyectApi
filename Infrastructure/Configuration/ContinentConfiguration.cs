using Domain.Entities.Continents;
using Domain.ValueObject.Continents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public sealed class ContinentConfiguration : IEntityTypeConfiguration<Continent>
{
    public void Configure(EntityTypeBuilder<Continent> builder)
    {
        builder.ToTable("continents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new ContinentId(value))
            .IsRequired();

        builder.Property(x => x.Name)
            .HasConversion(
                name => name.Value,
                value => new ContinentName(value))
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.CreateAt)
            .HasColumnName("create_at")
            .IsRequired();
    }
}