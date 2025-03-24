using System.Net;

namespace NLPC.PCMS.Common.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(message, HttpStatusCode.NotFound)
        {

        }
    }
}
