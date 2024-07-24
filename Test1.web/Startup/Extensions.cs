using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Test.Core.Interface;
using Test.Core.Services;
using Test.Infrastructure;

namespace Test.Web.Startup
{
    public static  class Extensions
    {

        public static IServiceCollection RegisterServices( this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddControllersWithViews();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IReminderRepository, ReminderRepository>();
            services.AddRazorPages();
            return services;    
        }
    }
}
