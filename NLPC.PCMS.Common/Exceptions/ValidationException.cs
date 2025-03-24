using System.Net;

namespace NLPC.PCMS.Common.Exceptions
{
    public class ValidationException : BaseException
    {
        public ValidationException(string message) : base(message, HttpStatusCode.BadRequest)
        {
            Errors = new List<string>() { message };
        }

        public ValidationException(string message, List<string> errors) : base(message, errors, HttpStatusCode.BadRequest)
        {

        }
    }
}
