using Mware.CollegeDreams.Infrastructure.Persistence;

namespace NLPC.PCMS.Api.StartupExtentions
{
    public static class DBContextConfig
    {
        public static IServiceCollection AddDBContextExtension(this IServiceCollection services, IConfiguration Configuration)
        {
            var connString = Configuration.GetConnectionString("ConnectionString");
            //services.AddDbContext<AppDBContext>(options => options.UseNpgsql(connString!));
            return services;
        }
    }

}
