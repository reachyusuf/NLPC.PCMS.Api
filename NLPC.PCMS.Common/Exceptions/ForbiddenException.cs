using System.Net;

namespace NLPC.PCMS.Common.Exceptions
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message) : base(message, HttpStatusCode.Forbidden)
        {

        }
    }
}
