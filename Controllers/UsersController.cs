using System.Net;
using cusho.Dtos;
using cusho.Dtos.UserDtos;
using cusho.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cusho.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(UsersService usersService) : ControllerBase
{

    [HttpGet("{userId}", Name = nameof(GetUserById))]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> GetUserById(long userId)
    {
        var response = await usersService.GetUserByIdAsync(userId);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> GetAllUsers()
    {
        var response = await usersService.GetAllUsersAsync();

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }

    //should do it for currently registered user
    [Authorize]
    [HttpPost("change-password")]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> ChangePassword(
        [FromBody] ChangePasswordDto changePasswordDto)
    {
        var response = await usersService.ChangePasswordAsync(changePasswordDto);

        if (!response.Success)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return NoContent();
    }
}