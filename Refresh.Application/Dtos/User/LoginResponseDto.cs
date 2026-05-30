namespace Refresh.Application.Dtos;

public class LoginResponseDto
{
    public string? Username { get; set; }
    public string? AccessToken { get; set; }
    public DateTime? ExpiresIn { get; set; }
    public string? RefreshToken { get; set; }
}