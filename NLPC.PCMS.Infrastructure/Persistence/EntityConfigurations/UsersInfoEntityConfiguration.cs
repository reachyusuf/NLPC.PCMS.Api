using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLPC.PCMS.Domain.Entities;

namespace NLPC.PCMS.Infrastructure.Persistence.EntityConfigurations
{
    public class UsersInfoEntityConfiguration : IEntityTypeConfiguration<UsersEntity>
    {
        public void Configure(EntityTypeBuilder<UsersEntity> builder)
        {
            builder.Property(a => a.Id).HasMaxLength(50);
            builder.Property(a => a.UserName).HasMaxLength(50).IsRequired();
            builder.Property(a => a.Email).HasMaxLength(100).IsRequired();
            builder.Property(a => a.PhoneNumber).HasMaxLength(50);
            builder.Property(a => a.Role).HasMaxLength(50).IsRequired();
            builder.Property(a => a.ProfileName).HasMaxLength(100);
        }
    }
}
