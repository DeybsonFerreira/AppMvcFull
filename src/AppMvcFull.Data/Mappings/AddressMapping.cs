using AppMvcFull.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppMvcFull.Data.Mappings
{
    internal class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.StreetName)
            .IsRequired()
            .HasColumnType("varchar(100)");

            builder.Property(x => x.Number)
            .IsRequired()
            .HasColumnType("varchar(50)");

            builder.Property(x => x.ZipCode)
            .IsRequired()
            .HasColumnType("varchar(8)");

            builder.Property(x => x.ZipCode)
            .IsRequired()
            .HasColumnType("varchar(100)");

            builder.Property(x => x.City)
            .IsRequired()
            .HasColumnType("varchar(50)");

            builder.Property(x => x.State)
            .IsRequired()
            .HasColumnType("varchar(50)");

            builder.Property(x => x.District)
            .IsRequired()
            .HasColumnType("varchar(50)");

            builder.ToTable("Addresses");

        }
    }
}
