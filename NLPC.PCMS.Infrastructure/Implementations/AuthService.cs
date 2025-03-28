using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLPC.PCMS.Application.Interfaces;
using NLPC.PCMS.Application.Interfaces.Repositories;
using NLPC.PCMS.Common.DTOs;
using NLPC.PCMS.Common.DTOs.Request;
using NLPC.PCMS.Common.DTOs.Response;
using NLPC.PCMS.Common.Enums;
using NLPC.PCMS.Common.Exceptions;
using NLPC.PCMS.Common.Utilities;
using NLPC.PCMS.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NLPC.PCMS.Infrastructure.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<UsersEntity> _userManager;
        private readonly SignInManager<UsersEntity> _signInManager;
        private readonly AppSettingsDto _appSettings;
        private readonly IUnitofWork _unitofwork;

        public AuthService(IOptions<AppSettingsDto> appSettingsJson, UserManager<UsersEntity> _userManager, SignInManager<UsersEntity> _signInManager, IUnitofWork _unitofwork)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._unitofwork = _unitofwork;
            this._appSettings = appSettingsJson.Value ?? throw new ApiException("could not find or bind appsettings.json file");
        }

        public async Task<GenericResponseDto<LoginResponseDto>> Login(LoginDto model)
        {
            var predicate = PredicateBuilder.True<UsersEntity>();
            predicate = predicate.And(i => i.UserName!.ToLower() == model.Username.ToLower());

            var user = await _unitofwork.Repository<UsersEntity>().Find(filter: predicate, includePaths: new List<string>() { "School" });
            if (user is null)
                throw new ValidationException("Invalid username or password");

            var _authResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (_authResult?.IsLockedOut is true)
                throw new ValidationException("Sorry! account is locked due to repeated failed login attempts");

            if (_authResult?.Succeeded is false)
                throw new ValidationException("Invalid username or password");

            var response = GenerateSecurityToken(user);
            return GenericResponseDto<LoginResponseDto>.ReturnSuccess(response);
        }

        public async Task<GenericResponseDto<bool>> Register(RegisterDto model)
        {
            var user = await _unitofwork.Repository<UsersEntity>().Find(filter: a => a.Email == model.Email);
            if (user is not null)
                throw new ValidationException("Oops sorry user already exist");

            user = new UsersEntity()
            {
                Email = model!.Email,
                CreatedDate = DateTime.UtcNow,
                ProfileName = model!.ProfileName,
                Role = UserRoles.User.ToString(),
                UserName = model!.Username,
            };

            var response = await _userManager.CreateAsync(user, model!.Password);
            if (response?.Succeeded is false)
            {
                var errMsg = new List<string>();
                foreach (var item in response.Errors)
                {
                    errMsg.Add(item.Description);
                }
                throw new ValidationException(errMsg.FirstOrDefault()!);
            }

            return GenericResponseDto<bool>.ReturnSuccess(true);
        }

        private LoginResponseDto GenerateSecurityToken(UsersEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Jwt.JwtSecretKey);
            SecurityTokenDescriptor? tokenDescriptor = null;
            tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                                                {
                        new Claim(ClaimTypes.Email, user.Email!),
                        new Claim(ClaimTypes.Name, user.UserName!),
                        new Claim(ClaimTypes.NameIdentifier, user.UserName!),
                        new Claim(ClaimTypes.Role, user.Role),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("ProfileName", user.ProfileName)
                    }),
                Expires = DateTime.UtcNow.AddMinutes(_appSettings.Jwt.JwtTokenExpiredTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _appSettings.Jwt.JwtIssuer,
                Issuer = _appSettings.Jwt.JwtIssuer
            };


            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var UserToken = tokenHandler.WriteToken(securityToken);
            var authResponseModel = new LoginResponseDto() { Token = UserToken };
            return authResponseModel;
        }
    }
}
