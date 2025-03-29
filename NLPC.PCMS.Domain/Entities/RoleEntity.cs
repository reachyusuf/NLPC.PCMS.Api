using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace NLPC.PCMS.Domain.Entities
{
    [Table("Role")]
    public class RoleEntity : IdentityRole<string>
    {

    }
}
