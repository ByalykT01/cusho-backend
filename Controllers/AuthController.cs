using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using cusho.Dtos;
using cusho.Dtos.UserDtos;
using cusho.Models;
using cusho.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace cusho.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthService authService, IConfiguration config) : ControllerBase
{
    
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> RegisterUser(UserRegistrationDto userRegistrationDto)
    {
        var response = await authService.RegisterUserAsync(userRegistrationDto);
        if (response.StatusCode != HttpStatusCode.Created)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return CreatedAtRoute(nameof(UsersController.GetUserById), new { userId = response.Data?.Id }, response);
    }
    
    [HttpPost("login")]
    public IResult Login([FromBody] LoginDto loginDto)
    {
        if (!authService.ValidateUser(loginDto))
            return Results.Unauthorized();

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, loginDto.Email),
            new Claim(ClaimTypes.Role, nameof(Role.User)),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return Results.Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }
}