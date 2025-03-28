using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace NLPC.PCMS.Domain.Entities
{
    [Table("Role")]
    public class RoleEntity : IdentityRole
    { }
}
