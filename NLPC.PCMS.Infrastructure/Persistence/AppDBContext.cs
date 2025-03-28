using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NLPC.PCMS.Domain.Entities;
using NLPC.PCMS.Infrastructure.Persistence.EntityConfigurations;

namespace Mware.CollegeDreams.Infrastructure.Persistence
{
    public class AppDBContext : IdentityDbContext<UsersEntity, RoleEntity, string>
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
            //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ApplyEntityConfigurations(builder);
        }

        private void ApplyEntityConfigurations(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UsersInfoEntityConfiguration());
        }
    }
}
