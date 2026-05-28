using Refresh.Application.Dtos;

namespace Refresh.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsers();
    Task<UserDto> GetUserById(int userId);
    Task<UserDto> GetUserByEmail(string email);
}