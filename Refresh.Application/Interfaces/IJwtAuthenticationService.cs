using Refresh.Application.Dtos;
using Refresh.Core.Entities;

namespace Refresh.Application.Interfaces;

public interface IJwtAuthenticationService
{
    Task<LoginResponseDto?> Login(LoginRequestDto req);
    Task<LoginResponseDto?> ValidateRefreshToken(string token);
    Task<LoginResponseDto> GenerateJwtToken(User user);
    Task<string> GenerateRefreshToken(int userId);
}