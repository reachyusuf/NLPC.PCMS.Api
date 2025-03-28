namespace NLPC.PCMS.Common.DTOs
{
    public class GenericResponseDto<T>
    {
        public bool IsSuccessful { get; set; } = false;
        public string Message { get; set; }
        public T Result { get; set; } = default(T)!;
        public List<string> Errors { get; set; } = new List<string>();

        public static GenericResponseDto<T> ReturnSuccess(T result, string? message = null, List<string>? messages = null)
        {
            return new GenericResponseDto<T>() { IsSuccessful = true, Message = message ?? Constants.Constants.successMessage, Result = result };
        }

        public static GenericResponseDto<T> ReturnBadRequest(string? message = null, List<string>? messages = null)
        {
            var result = new GenericResponseDto<T>() { Message = message ?? Constants.Constants.failedMessage, Errors = messages ?? new List<string>() { message ?? Constants.Constants.successMessage.ToString() }, Result = default(T)! };

            if (string.IsNullOrEmpty(message) && messages != null)
                result.Message = messages.FirstOrDefault()!;

            if (messages == null && !string.IsNullOrEmpty(message))
                result.Errors = new List<string>() { message };

            return result;
        }

        public static GenericResponseDto<T> ReturnUnAuthorizedRequest(string? message = null, List<string>? messages = null)
        {
            var result = new GenericResponseDto<T>() { Message = message ?? Constants.Constants.unAuthorisedMessage, Errors = messages ?? new List<string>() { message ?? Constants.Constants.unAuthorisedMessage }, Result = default(T)! };
            if (string.IsNullOrEmpty(message) && messages != null)
                result.Message = messages.FirstOrDefault()!;

            if (messages == null && !string.IsNullOrEmpty(message))
                result.Errors = new List<string>() { message };

            return result;
        }
       
        public static GenericResponseDto<T> ReturnForbiddenResponse(string? message = null, List<string>? messages = null)
        {
            var result = new GenericResponseDto<T>() { Message = message ?? Constants.Constants.failedMessage, Errors = messages ?? new List<string>() { message ?? Constants.Constants.failedMessage }, Result = default(T)! };
            if (string.IsNullOrEmpty(message) && messages is not null)
                result.Message = messages.FirstOrDefault()!;
            if (messages == null && !string.IsNullOrEmpty(message))
                result.Errors = new List<string>() { message };

            return result;
        }
    }

}
