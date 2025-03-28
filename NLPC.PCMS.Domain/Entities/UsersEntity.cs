using Microsoft.AspNet.Identity.EntityFramework;
using NLPC.PCMS.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace NLPC.PCMS.Domain.Entities
{
    [Table("Users")]
    public class UsersEntity : IdentityUser
    {
        public UsersEntity()
        {

        }


        public string Role { get; set; } = UserRoles.User.ToString();
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;       
        public string ProfileName { get; set; } = string.Empty;
    }
}
