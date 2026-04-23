using System.Security.Claims;
using cusho.Dtos;
using cusho.Dtos.UserDtos;
using cusho.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace cusho.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(UsersService usersService) : ControllerBase
{
    [HttpGet("/{userId}", Name = nameof(GetUserById))]
    public async Task<Results<
        Ok<UserResponseDto>,
        BadRequest<string>
    >> GetUserById(Guid userId)
    {
        var result = await usersService.GetUserByIdAsync(userId);

        if (result.IsFailure)
        {
            return TypedResults.BadRequest(result.Error);
        }

        return TypedResults.Ok(result.Value);
    }

    [Authorize("IsAdmin")]
    [HttpGet]
    public async Task<Results<
        Ok<List<UserResponseDto>>,
        BadRequest<string>
    >> GetUsers()
    {
        var result = await usersService.GetAllUsersAsync();

        if (result.IsFailure)
        {
            return TypedResults.BadRequest(result.Error);
        }

        return TypedResults.Ok(result.Value);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<Results<
        NoContent,
        BadRequest<string>
    >> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        var email = User.FindFirst(ClaimTypes.Name)?.Value!;

        var result = await usersService.ChangePasswordAsync(email, changePasswordDto);

        if (result.IsFailure)
        {
            return TypedResults.BadRequest(result.Error);
        }

        return TypedResults.NoContent();
    }
}