using Microsoft.AspNetCore.Authorization;

namespace NLPC.PCMS.Api.Controllers
{
    [Authorize]
    public class BaseProtectedController : BaseController
    {

    }
}
