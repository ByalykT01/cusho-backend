using cusho.Dtos.UserDtos;
using cusho.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace cusho.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<Results<
            CreatedAtRoute<UserResponseDto>,
            BadRequest<string>
        >> RegisterUser(UserRegistrationDto userRegistrationDto)
    {
        var result = await authService.RegisterUserAsync(userRegistrationDto);

        if (result.IsFailure)
        {
            return TypedResults.BadRequest(result.Error);
        }

        return TypedResults.CreatedAtRoute(result.Value, nameof(UsersController.GetUserById), new { userId = result.Value.Id });
    }

    [HttpPost("login")]
    public async Task<Results<
            Ok<LoginResponseDto>,
            UnauthorizedHttpResult
        >> Login(LoginDto loginDto)
    {
        var result = await authService.LoginAsync(loginDto);

        if (result.IsFailure)
        {
            return TypedResults.Unauthorized();
        }

        return TypedResults.Ok(result.Value);
    }
}