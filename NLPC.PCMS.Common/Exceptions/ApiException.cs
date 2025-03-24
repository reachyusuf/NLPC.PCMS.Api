namespace NLPC.PCMS.Common.Exceptions
{
    public class ApiException : BaseException
    {
        public ApiException(string message) : base(message)
        {
            Errors = new List<string>() { message };
        }
    }
}
