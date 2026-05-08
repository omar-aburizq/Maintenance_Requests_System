using Application.Services.AuthService.DTOs;

namespace Application.Services.AuthService
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginRequestDto input);
        Task ChangeUserPassword(ChangeUserPasswordDto input);
        Task<string> RefreshToken(RefreshTokenDto input);
    }
}
