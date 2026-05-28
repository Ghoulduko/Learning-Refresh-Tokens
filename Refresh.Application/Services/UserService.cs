using Refresh.Application.Dtos;
using Refresh.Application.Interfaces;
using Refresh.Core.Exceptions;
using Refresh.Core.Interfaces;

namespace Refresh.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsers();
        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
        });
    }

    public async Task<UserDto> GetUserById(int userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user is null)
            throw new UserNotFoundException($"No user with id: {userId} was found.");
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
        };
    }

    public async Task<UserDto> GetUserByEmail(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);
        if (user is null)
            throw new UserNotFoundException($"No user with email: {email} was found.");
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
        };
    }
}