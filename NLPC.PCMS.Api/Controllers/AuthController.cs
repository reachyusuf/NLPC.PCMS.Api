using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLPC.PCMS.Application.Interfaces;
using NLPC.PCMS.Common.DTOs;
using NLPC.PCMS.Common.DTOs.Request;
using NLPC.PCMS.Common.DTOs.Response;

namespace NLPC.PCMS.Api.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService _authService)
        {
            this._authService = _authService;
        }

        [ProducesResponseType(typeof(GenericResponseDto<LoginResponseDto>), 200)]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var response = await _authService.Login(model);
            return Ok(response);
        }

        [ProducesResponseType(typeof(GenericResponseDto<bool>), 200)]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var response = await _authService.Register(model);
            return Ok(response);
        }
    }
}
