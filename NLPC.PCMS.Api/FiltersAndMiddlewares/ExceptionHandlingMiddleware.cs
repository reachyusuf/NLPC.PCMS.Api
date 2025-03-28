using NLPC.PCMS.Common.DTOs;
using NLPC.PCMS.Common.Exceptions;
using NLPC.PCMS.Common.Extensions;

namespace NLPC.PCMS.Api.FiltersAndMiddlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ExceptionHandlingMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                // Set the result to the default value of T based on its type
                var result = new GenericResponseDto<object>()
                {
                    Errors = new List<string>() { error?.Message! },
                    Message = error?.Message!,
                    Result = new()!
                };

                var statusCode = StatusCodes.Status500InternalServerError;

                // Handle specific exceptions as needed
                switch (error)
                {
                    case ValidationException ve:
                        statusCode = (int)ve.StatusCode;
                        result.Errors = ve?.Errors ?? new List<string>() { ve!.Message };
                        break;
                    case UnAuthorisedException ue:
                        statusCode = (int)ue.StatusCode;
                        result.Errors = ue?.Errors ?? new List<string>();
                        break;
                    case ForbiddenException fe:
                        statusCode = (int)fe.StatusCode;
                        result.Errors = fe?.Errors ?? new List<string>();
                        break;
                    case ApiException ae:
                        statusCode = (int)ae.StatusCode;
                        result.Errors = ae?.Errors ?? new List<string>() { ae!.Message };
                    
                        break;
                    case NotFoundException ne:
                        statusCode = (int)ne.StatusCode;
                        result.Errors = ne?.Errors ?? new List<string>() { ne!.Message };
                        break;
                }

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _logger = scope.ServiceProvider.GetService<ILogger<ExceptionHandlingMiddleware>>();
                    _logger!.LogError(result.Serialize());
                }

                response.StatusCode = statusCode;
                await response.WriteAsync(result.Serialize());
            }
        }
    }
}
