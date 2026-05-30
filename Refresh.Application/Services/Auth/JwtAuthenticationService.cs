using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Refresh.Application.Dtos;
using Refresh.Application.Interfaces;
using Refresh.Core.Entities;
using Refresh.Core.Interfaces;

namespace Refresh.Application.Services;

public class JwtAuthenticationService : IJwtAuthenticationService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    
    public JwtAuthenticationService(IConfiguration configuration, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LoginResponseDto?> Login(LoginRequestDto req)
    {
        if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
            return null;
        
        var user = await _userRepository.GetUserByEmail(req.Email);
        if (user is null || !BC.Verify(req.Password, user.PasswordHash))
            return null;

        return await GenerateJwtToken(user);
    }

    public async Task<LoginResponseDto?> ValidateRefreshToken(string token)
    {
        var refreshToken = await _refreshTokenRepository.GetRefreshToken(token);
        if (refreshToken is null || refreshToken.Expires < DateTime.UtcNow)
            return null;
        
        await _refreshTokenRepository.Delete(refreshToken);
        
        var user = await _userRepository.GetUserById(refreshToken.UserId);
        if (user is null) 
            return null;
        
        return await GenerateJwtToken(user);
    }

    public async Task<LoginResponseDto> GenerateJwtToken(User user)
    {
        var issuer = _configuration["JwtConfig:Issuer"];
        var audience = _configuration["JwtConfig:Audience"];
        var key = Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]);
        var tokenValidityMins = int.Parse(_configuration["JwtConfig:TokenValidityMins"]);
        var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

        var claims = new List<Claim>
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("Email", user.Email),
            new Claim("Username", user.Username),
        };
        
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims: claims,
            expires: tokenExpiryTimeStamp,
            signingCredentials: credentials
        );
        
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new LoginResponseDto
        {
            Username = user.Username,
            AccessToken = accessToken,
            ExpiresIn = tokenExpiryTimeStamp,
            RefreshToken = await GenerateRefreshToken(user.Id)
        };
    }

    public async Task<string> GenerateRefreshToken(int userId)
    {
        var refreshTokenValidityMins = int.Parse(_configuration["JwtConfig:RefreshTokenValidity"]);
        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            Expires = DateTime.UtcNow.AddMinutes(refreshTokenValidityMins),
            UserId = userId,
        };
        
        await _refreshTokenRepository.AddRefreshToken(refreshToken);
        return refreshToken.Token;
    }
    
}