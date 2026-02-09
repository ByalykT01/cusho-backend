using System.Net;
using cusho.Data;
using cusho.Dtos;
using cusho.Dtos.UserDtos;
using cusho.Models;
using cusho.CustomExceptions;
using Microsoft.EntityFrameworkCore;

namespace cusho.Services;

public class UserService(ApplicationDbContext dbContext)
{
    public async Task<ApiResponse<UserResponseDto>> RegisterUserAsync(UserRegistrationDto userRegistrationDto)
    {
        try
        {
            var normalizedEmail = userRegistrationDto.Email.ToLowerInvariant();

            if (await dbContext.Users.AnyAsync(u =>
                    u.Email == normalizedEmail))
            {
                return new ApiResponse<UserResponseDto>(HttpStatusCode.BadRequest, "Email is already taken");
            }

            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            var cart = new Cart();
            dbContext.Carts.Add(cart);
            await dbContext.SaveChangesAsync();

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
        catch (DbUpdateException ex) when (DbExceptions.IsUniqueConstraintViolation(ex))
        {
            return new ApiResponse<UserResponseDto>(HttpStatusCode.BadRequest, "Email is already taken");
        }
        catch (Exception e)
        {
            // return new ApiResponse<UserResponseDto>(HttpStatusCode.InternalServerError,
            //     $"An server error occured: {e.Message}");
            return new ApiResponse<UserResponseDto>(HttpStatusCode.InternalServerError,
                "An unexpected error occurred. Please try again later.");
        }
    }

    public async Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(long userId)
    {
        try
        {
            if (userId <= 0)
            {
                return new ApiResponse<UserResponseDto>(HttpStatusCode.BadRequest, "User id is a positive number");
            }

            var foundUser = await dbContext.Users.Where(u => u.Id == userId)
                .Select(u => new UserResponseDto()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                })
                .FirstOrDefaultAsync();

            return foundUser == null
                ? new ApiResponse<UserResponseDto>(HttpStatusCode.NotFound, "User not found")
                : new ApiResponse<UserResponseDto>(HttpStatusCode.OK, foundUser);
        }
        catch (Exception e)
        {
            // return new ApiResponse<UserResponseDto>(HttpStatusCode.InternalServerError,
            //     $"An server error occured: {e.Message}");
            return new ApiResponse<UserResponseDto>(HttpStatusCode.InternalServerError,
                "An unexpected error occurred. Please try again later.");
        }
    }
}