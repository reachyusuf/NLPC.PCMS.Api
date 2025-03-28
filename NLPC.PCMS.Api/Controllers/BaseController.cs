using Microsoft.AspNetCore.Mvc;

namespace NLPC.PCMS.Api.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        //protected const string InvalidParamBadRequest = "Invalid parameters";

        //protected IActionResult ProcessResponse<T>(GenericResponse<T> result)
        //{
        //    switch (result.Status)
        //    {
        //        case ResponseStatus.Successful:
        //        case ResponseStatus.UnverifiedUser:
        //        case ResponseStatus.InProgress:
        //            return Ok(result);
        //        case ResponseStatus.Unauthorized:
        //            return Unauthorized(result);
        //        case ResponseStatus.Forbidden:
        //            return Forbid();
        //        default:
        //            return BadRequest(result);
        //    }
        //}

        //protected IActionResult ReturnValidationError(string message)
        //{
        //    ResponseResult<string> result = new ResponseResult<string>() { Message = message, Result = message, Status = ResponseStatus.BadRequest, Errors = new List<string>() { message } };
        //    return BadRequest(result);
        //}

        //protected IActionResult ReturnSuccess<T>(T result, string? message = null, List<string>? errors = null)
        //{
        //    return Ok(new ResponseResult<T>() { Result = result, Status = ResponseStatus.Successful, Message = message ?? ResponseStatus.Successful.ToString() });
        //}

        //protected IActionResult ReturnSuccess(dynamic result, string? message = null, List<string>? errors = null)
        //{
        //    return Ok(new ResponseResult<dynamic>() { Result = result, Status = ResponseStatus.Successful, Message = message ?? ResponseStatus.Successful.ToString() });
        //}

        //protected IActionResult ReturnBadRequest(string? message = null, List<string>? errors = null)
        //{
        //    return BadRequest(new ResponseResult<dynamic>() { Status = ResponseStatus.BadRequest, Message = message ?? ResponseStatus.BadRequest.ToString(), Errors = errors ?? new List<string>() { message ?? ResponseStatus.BadRequest.ToString() } });
        //}
    }
}
