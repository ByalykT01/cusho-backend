using System.Net;
using cusho.Dtos;
using cusho.Dtos.UserDtos;
using cusho.Services;
using Microsoft.AspNetCore.Mvc;

namespace cusho.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> RegisterUser(UserRegistrationDto userRegistrationDto)
    {
        var response = await userService.RegisterUserAsync(userRegistrationDto);
        if (response.StatusCode != HttpStatusCode.Created)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return CreatedAtRoute(nameof(GetUserById), new { userId = response.Data?.Id }, response);
    }

    [HttpGet("{userId}", Name = nameof(GetUserById))]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> GetUserById(long userId)
    {
        var response = await userService.GetUserByIdAsync(userId);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }
}