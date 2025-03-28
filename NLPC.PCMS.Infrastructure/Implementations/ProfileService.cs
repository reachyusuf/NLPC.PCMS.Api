using NLPC.PCMS.Application.Interfaces;
using NLPC.PCMS.Common.DTOs;

namespace NLPC.PCMS.Infrastructure.Implementations
{
    public class ProfileService : IProfileService
    {
        public async Task<GenericResponseDto<string>> GetProfileDetails()
        {
            return GenericResponseDto<string>.ReturnSuccess("Successful mock result");
        }
    }
}
