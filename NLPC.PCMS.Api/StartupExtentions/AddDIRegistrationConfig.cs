using NLPC.PCMS.Application.Interfaces;
using NLPC.PCMS.Application.Interfaces.Repositories;
using NLPC.PCMS.Common.DTOs;
using NLPC.PCMS.Infrastructure.Implementations;
using NLPC.PCMS.Infrastructure.Persistence.Repository;

namespace NLPC.PCMS.Api.StartupExtentions
{
    public static class AddDIRegistrationConfig
    {
        public static IServiceCollection AddDIRegistrationExtension(this IServiceCollection services, IConfiguration Configuration, string _envName, AppSettingsDto _appSettings)
        {
            var appSettingsConfig = Configuration.GetSection("AppSettings");
            services.Configure<AppSettingsDto>(appSettingsConfig);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitofWork, UnitofWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProfileService, ProfileService>();

            return services;
        }
    }
}
