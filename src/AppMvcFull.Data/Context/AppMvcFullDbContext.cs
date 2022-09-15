using AppMvcFull.Business.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AppMvcFull.Data.Context
{
    public class AppMvcFullDbContext : DbContext
    {
        public AppMvcFullDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppMvcFullDbContext).Assembly);

            DisableAllDeleteCascate(builder);
            SetDefaultVarcharMax(builder);
            base.OnModelCreating(builder);
        }

        /// <summary>
        /// Desabilitar cascade delete
        /// </summary>
        /// <param name="builder"></param>
        private static void DisableAllDeleteCascate(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(c => c.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
        private static void SetDefaultVarcharMax(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(c => c.GetProperties()
                .Where(d => d.ClrType == typeof(string))))
                property.SetPrecision(200); //200 max
        }

    }
}
