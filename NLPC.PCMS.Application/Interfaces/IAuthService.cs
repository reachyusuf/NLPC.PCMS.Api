using NLPC.PCMS.Common.DTOs;
using NLPC.PCMS.Common.DTOs.Request;
using NLPC.PCMS.Common.DTOs.Response;

namespace NLPC.PCMS.Application.Interfaces
{
    public interface IAuthService
    {
        Task<GenericResponseDto<LoginResponseDto>> Login(LoginDto model);
        Task<GenericResponseDto<bool>> Register(RegisterDto model);
    }
}
