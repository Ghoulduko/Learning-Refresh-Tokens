using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Refresh.Application.Dtos;
using Refresh.Application.Interfaces;

namespace Learning_Refresh_Tokens.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
   private readonly IUserService _userService;

   public UserController(IUserService userService)
   {
      _userService = userService;
   }

   [HttpGet("GetAllUsers")]
   public async Task<Ok<IEnumerable<UserDto>>> GetAllUsers()
   {
      return TypedResults.Ok(await _userService.GetAllUsers());
   }

   [HttpGet("GetLoggedInUser")]
   public async Task<Ok<UserDto>> GetLoggedInUser()
   {
      var userId = User.FindFirst("Id")?.Value;
      return TypedResults.Ok(await _userService.GetUserById(int.Parse(userId)));
   }
   
   [HttpGet("GetUserById/{userId:int}")]
   public async Task<Ok<UserDto>> GetUserById(int userId)
   {
      return TypedResults.Ok(await _userService.GetUserById(userId));
   }
   
   [HttpGet("GetUserByEmail/{email}")]
   public async Task<Ok<UserDto>> GetUserByEmail(string email)
   {
      return TypedResults.Ok(await _userService.GetUserByEmail(email));
   }
}