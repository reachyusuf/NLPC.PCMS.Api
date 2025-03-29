using Mware.CollegeDreams.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace NLPC.PCMS.Api.StartupExtentions
{
    public static class DBContextConfig
    {
        public static IServiceCollection AddDBContextExtension(this IServiceCollection services, IConfiguration Configuration)
        {
            var connString = Configuration.GetConnectionString("ConnectionString");
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connString!));
            return services;
        }
    }

}
