namespace NLPC.PCMS.Common.DTOs.Request
{
    public class RegisterDto : LoginDto
    {
        public string Email { get; set; }
        public string ProfileName { get; set; }
    }
}
