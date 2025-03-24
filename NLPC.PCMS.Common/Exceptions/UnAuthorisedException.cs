using System.Net;

namespace NLPC.PCMS.Common.Exceptions
{
    public class UnAuthorisedException : BaseException
    {
        public UnAuthorisedException(string message) : base(message, HttpStatusCode.Unauthorized)
        {

        }
    }
}
