using Microsoft.AspNetCore.Cors.Infrastructure;
using NLPC.PCMS.Application.Interfaces.Repositories;
using NLPC.PCMS.Application.Interfaces;
using NLPC.PCMS.Common.DTOs;
using NLPC.PCMS.Common.Exceptions;
using NLPC.PCMS.Common.Extensions;
using NLPC.PCMS.Infrastructure.Implementations;
using NLPC.PCMS.Infrastructure.Persistence.Repository;
using StackExchange.Redis;
using System.Runtime.InteropServices.JavaScript;

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
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IJsonSerializer, JsonSerializer>();

            services.AddScoped<IUnitofWork, UnitofWork>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IImageRandomName, ImageRandomName>();
            services.AddScoped<ICalendarCategory, CalendarCategoryService>();
            services.AddScoped<IClass, ClassService>();
            services.AddScoped<IDesignation, DesignationService>();
            services.AddScoped<IEmployee, EmployeeService>();
            services.AddScoped<IExamType, ExamTypeService>();
            services.AddScoped<IForm, FormService>();
            services.AddScoped<IGrade, GradeService>();
            services.AddScoped<ILgaService, LgaService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<INationality, NationalityService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IQualification, QualificationService>();
            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<ISection, SectionService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IStudentAdmission, StudentAdmissionService>();
            services.AddScoped<IStudentDetails, StudentDetailsService>();
            services.AddScoped<ISubject, SubjectService>();
            services.AddScoped<IUserProfile, UserProfileService>();
            services.AddScoped<IReportService, ReportService>();
            //--

            //--new dapper repos
            services.AddScoped<IClassDapperRepository, ClassDapperRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();

            //--Dapper DIs
            services.AddScoped<IClassDPRepository, ClassDPRepository>();
            services.AddScoped<IEmployeeDPRepository, EmployeeDPRepository>();
            services.AddScoped<IExamTypeDPRepository, ExamTypeDPRepository>();
            services.AddScoped<ISchoolDPRepository, SchoolDPRepository>();
            services.AddScoped<IStudentDetailsDPRepository, StudentDetailsDPRepository>();
            services.AddScoped<ITermService, TermService>();
            services.AddScoped<IOnboardingService, OnboardingService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISchoolCalendarService, SchoolCalendarService>();

            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<IFileUploadContaboAwsService, FileUploadContaboAwsService>();
            services.AddScoped<IDataEncryptionService, DataEncryptionService>();
            services.AddScoped<IStudentResultService, StudentResultService>();
            services.AddScoped<IProccessService, ProccessService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IDataSeedService, DataSeedService>();

            //--DI for account related modules
            services.AddScoped<IPaymentItemService, PaymentItemService>();
            services.AddScoped<IBillTemplateService, BillTemplateService>();
            services.AddScoped<IBillService, BillService>();

            // Configure Redis for all environments (not just production)
            services.AddScoped<IRedisService, RedisService>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = _appSettings!.Redis.ConnectionString;
                options.InstanceName = $"{_appSettings.AppName}-{_appSettings.Redis.InstanceName}-";
            });

            // Validate configuration
            if (string.IsNullOrEmpty(_appSettings.Redis.ConnectionString))
                throw new ApiException("Redis configuration is missing");

            // Configure Redis
            var redisConfig = ConfigurationOptions.Parse(_appSettings.Redis.ConnectionString);
            redisConfig.AbortOnConnectFail = false;
            redisConfig.ConnectTimeout = 5000;
            redisConfig.SyncTimeout = 2000;
            redisConfig.AsyncTimeout = 2000;

            // Environment-specific tuning
            if (_envName.IsEqualTo("Production") is true)
            {
                redisConfig.ReconnectRetryPolicy = new LinearRetry(1000);
                redisConfig.KeepAlive = 30;
                redisConfig.ConfigCheckSeconds = 60;
            }
            else
            {
                redisConfig.ReconnectRetryPolicy = new LinearRetry(500);
                redisConfig.AllowAdmin = true;
                redisConfig.ConnectTimeout = 3000;
            }

            // Register with logging
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                //var logger = sp.GetRequiredService<ILogger>();
                var muxer = ConnectionMultiplexer.Connect(redisConfig);

                //muxer.ConnectionFailed += (_, e) =>
                //    logger.Error(e.Exception!, "Redis connection failed");

                //muxer.ConnectionRestored += (_, e) =>
                //    logger.Information("Redis connection restored");

                //muxer.ErrorMessage += (_, e) =>
                //    logger.Error($"Redis server error: {e.Message}");

                return muxer;
            });

            return services;
        }

    }
}
