using HRMS.Repositories.Implementations;
using HRMS.Repositories.Interfaces;
using HRMS.Services.Implementations;
using HRMS.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILeaveBalanceRepository, LeaveBalanceRepository>();
            services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ILeaveService, LeaveService>();

            return services;
        }
    }
}
