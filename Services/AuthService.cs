using cusho.Common;
using cusho.Configuration;
using cusho.CustomExceptions;
using cusho.Data;
using cusho.Dtos.UserDtos;
using cusho.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace cusho.Services;

public class AuthService(ApplicationDbContext dbContext, IOptions<JwtSettings> jwtOptions)
{
    public async Task<Result<UserResponseDto>> RegisterUserAsync(UserRegistrationDto userRegistrationDto)
    {
        
        var normalizedEmail = userRegistrationDto.Email.ToLowerInvariant();

        var cart = new Cart();
        
        dbContext.Carts.Add(cart);
        
        var user = new User()
        {
            FirstName = userRegistrationDto.FirstName.Trim(),
            LastName = userRegistrationDto.LastName.Trim(),
            Email = normalizedEmail,
            Cart = cart,
            Password = BCrypt.Net.BCrypt.HashPassword(userRegistrationDto.Password),
            Created = DateTime.UtcNow
        };
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var userResponseDto = new UserResponseDto()
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
        };
        return userResponseDto;
    }

    public async Task<Result<LoginResponseDto>> LoginAsync(LoginDto loginDto)
    {
        var foundUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (foundUser is null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, foundUser.Password))
        {
            return Result<LoginResponseDto>.Failure("Invalid email or password");
        }

        var token = GenerateJwtToken(foundUser);

        var loginResponse = new LoginResponseDto
        {
            Id = foundUser.Id,
            FirstName = foundUser.FirstName,
            Token = token
        };

        return loginResponse;
    }

    private string GenerateJwtToken(User user)
    {
        var settings = jwtOptions.Value;
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, nameof(Role.User)),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));
        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
        
        // refresh
        //oauth

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}