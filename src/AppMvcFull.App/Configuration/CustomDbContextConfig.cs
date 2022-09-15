using AppMvcFull.App.Data;
using AppMvcFull.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppMvcFull.App.Configuration
{
    public static class CustomDbContextConfig
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration app)
        {
            var connectionString = app.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddDbContext<AppMvcFullDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}
