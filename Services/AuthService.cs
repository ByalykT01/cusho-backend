using System.Net;
using cusho.CustomExceptions;
using cusho.Data;
using cusho.Dtos;
using cusho.Dtos.UserDtos;
using cusho.Models;
using Microsoft.EntityFrameworkCore;

namespace cusho.Services;

public class AuthService(ApplicationDbContext dbContext)
{
    public async Task<ApiResponse<UserResponseDto>> RegisterUserAsync(UserRegistrationDto userRegistrationDto)
    {
        var normalizedEmail = userRegistrationDto.Email.ToLowerInvariant();

        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        var cart = new Cart();
        dbContext.Carts.Add(cart);
        await dbContext.SaveChangesAsync();

        try
        {
            var user = new User()
            {
                FirstName = userRegistrationDto.FirstName.Trim(),
                LastName = userRegistrationDto.LastName.Trim(),
                Email = normalizedEmail,
                CartId = cart.Id,
                Password = BCrypt.Net.BCrypt.HashPassword(userRegistrationDto.Password),
                Created = DateTime.UtcNow
            };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            var userResponseDto = new UserResponseDto()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
            return new ApiResponse<UserResponseDto>(HttpStatusCode.Created, userResponseDto);
        }
        catch (DbUpdateException e) when (DbExceptions.IsUniqueConstraintViolation(e))
        {
            return new ApiResponse<UserResponseDto>(HttpStatusCode.BadRequest, "Email is already taken");
        }
    }
    public bool ValidateUser(LoginDto loginDto)
    {
        var foundUser = dbContext.Users.FirstOrDefault(u => u.Email == loginDto.Email);
        if (foundUser is null) return false;
        return BCrypt.Net.BCrypt.Verify(loginDto.Password, foundUser.Password);
    }
}