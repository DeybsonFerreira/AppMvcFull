using AppMvcFull.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppMvcFull.Data.Mappings
{
    /// <summary>
    /// FluentAPI
    /// </summary>
    internal class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("varchar(100)");

            builder.Property(x => x.Description)
            .IsRequired()
            .HasColumnType("varchar(100)");

            builder.Property(x => x.Image)
            .IsRequired()
            .HasColumnType("varchar(100)");

            builder.ToTable("Products");
        }
    }

}
