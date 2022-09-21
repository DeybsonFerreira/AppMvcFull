using AppMvcFull.App.Data;
using AppMvcFull.Business.Interfaces;
using AppMvcFull.Business.Services;
using AppMvcFull.Data.Context;
using AppMvcFull.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace AppMvcFull.App.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddCustomInjection(this IServiceCollection services)
        {
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<AppMvcFullDbContext>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();

            services.AddScoped<INotification, NotificationServices>();
            services.AddScoped<ISupplierServices, SupplierServices>();
            services.AddScoped<IProductServices, ProductServices>();


            return services;
        }
    }
}
