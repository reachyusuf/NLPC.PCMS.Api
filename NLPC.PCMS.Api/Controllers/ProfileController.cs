using Microsoft.AspNetCore.Mvc;
using NLPC.PCMS.Application.Interfaces;
using NLPC.PCMS.Common.DTOs;

namespace NLPC.PCMS.Api.Controllers
{
    public class ProfileController : BaseProtectedController
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService _profileService)
        {
            this._profileService = _profileService;
        }

        [ProducesResponseType(typeof(GenericResponseDto<string>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetProfileDetails()
        {
            var response = await _profileService.GetProfileDetails();
            return Ok(response);
        }
    }
}