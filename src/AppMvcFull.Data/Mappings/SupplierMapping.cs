using AppMvcFull.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppMvcFull.Data.Mappings
{
    /// <summary>
    /// FluentAPI
    /// </summary>
    internal class SupplierMapping : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(x => x.DocumentNumber)
                .IsRequired()
                .HasColumnType("varchar(14)");

            // 1 : 1 => Fornecedor : Endereço
            builder.HasOne(x => x.Address)
                .WithOne(y => y.Supplier);

            // 1 : N => Produto : Fornecedor
            builder.HasMany(x => x.Products)
                .WithOne(y => y.Supplier)
                .HasForeignKey(p => p.SupplierId);

            builder.ToTable("Suppliers");
        }
    }
}