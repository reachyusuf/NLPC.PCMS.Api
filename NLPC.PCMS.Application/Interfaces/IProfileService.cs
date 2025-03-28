using NLPC.PCMS.Common.DTOs;

namespace NLPC.PCMS.Application.Interfaces
{
    public interface IProfileService
    {
        Task<GenericResponseDto<string>> GetProfileDetails();
    }
}
