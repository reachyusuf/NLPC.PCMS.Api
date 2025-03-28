using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Mware.CollegeDreams.Infrastructure.Persistence;
using NLPC.PCMS.Common.DTOs;
using NLPC.PCMS.Domain.Entities;
using System.Text;

namespace NLPC.PCMS.Api.StartupExtentions
{
    public static class AddIdentityManagerConfig
    {
        public static IServiceCollection AddIdentityManagerExtension(this IServiceCollection services, AppSettingsDto appSettings)
        {
            services.AddIdentity<UsersEntity, RoleEntity>(options =>
            {
                //--Signin
                options.SignIn.RequireConfirmedEmail = true;
                //options.SignIn.RequireConfirmedPhoneNumber = false;

                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings  
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;

                // User settings  
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDBContext>()
            .AddDefaultTokenProviders();

            services.AddTokenAuthentication(appSettings);

            return services;
        }

        private static IServiceCollection AddTokenAuthentication(this IServiceCollection services, AppSettingsDto _appSettingsJson)
        {
            var key = Encoding.ASCII.GetBytes(_appSettingsJson.Jwt.JwtSecretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _appSettingsJson.Jwt.JwtIssuer,
                    ValidAudience = _appSettingsJson.Jwt.JwtIssuer,

                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                };
            });

            return services;
        }
    }
}
