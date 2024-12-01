using webapi.DTOs;

namespace webapi.Services;

public interface IAuthService
{
    Task<AuthResponseDto> Login(LoginDto loginDto);
    Task<AuthResponseDto> Register(RegisterDto registerDto);
}
