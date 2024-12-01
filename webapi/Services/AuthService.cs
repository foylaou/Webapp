using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using webapi.Context;
using webapi.DTOs;
using webapi.Entities;

namespace webapi.Services;

public class AuthService : IAuthService
{
    private readonly Webapp _context;
    private readonly IConfiguration _configuration;

    public AuthService(Webapp context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> Register(RegisterDto registerDto)
    {
        // 檢查郵箱是否已存在
        if (await _context.UserInfos.AnyAsync(u => u.Email == registerDto.Email))
        {
            throw new Exception("Email already exists");
        }

        // 創建新用戶
        var user = new UserInfo
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password), // 使用 BCrypt 加密密碼
            CreatedAt = DateTime.UtcNow
        };

        _context.UserInfos.Add(user);
        await _context.SaveChangesAsync();

        // 生成 JWT Token
        return await GenerateToken(user);
    }

    public async Task<AuthResponseDto> Login(LoginDto loginDto)
    {
        var user = await _context.UserInfos
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        {
            throw new Exception("Invalid email or password");
        }

        return await GenerateToken(user);
    }

    private async Task<AuthResponseDto> GenerateToken(UserInfo user)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Email = user.Email,
            Expiration = token.ValidTo
        };
    }
}