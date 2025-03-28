using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLPC.PCMS.Common.DTOs;
using NLPC.PCMS.Common.Extensions;

namespace NLPC.PCMS.Api.FiltersAndMiddlewares
{
    public class ValidateFilterAttribute : ResultFilterAttribute
    {
        private readonly ILogger<ValidateFilterAttribute> _logger;

        public ValidateFilterAttribute(ILogger<ValidateFilterAttribute> _logger)
        {
            this._logger = _logger;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);

            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState;
                var allErrors = errors.Values.SelectMany(v => v.Errors);
                List<string> msg = new List<string>();
                foreach (var item in allErrors)
                {
                    msg.Add(item.ErrorMessage);
                }

                var result = new GenericResponseDto<string>() { Errors = msg, Message = msg.FirstOrDefault()!, Result = string.Empty };

                _logger.LogWarning(message: result.Serialize());
                context.Result = new BadRequestObjectResult(result);
            }
        }
    }

}
