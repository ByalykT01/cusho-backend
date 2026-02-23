using System.Net;
using cusho.CustomExceptions;
using cusho.Data;
using cusho.Dtos;
using cusho.Dtos.UserDtos;
using cusho.Models;
using Microsoft.EntityFrameworkCore;

namespace cusho.Services;

public class UsersService(ApplicationDbContext dbContext, ILogger<UsersService> logger)
{
    public async Task<ApiResponse<UserResponseDto>> RegisterUserAsync(UserRegistrationDto userRegistrationDto)
    {
        var normalizedEmail = userRegistrationDto.Email.ToLowerInvariant();

        if (await dbContext.Users.AnyAsync(u => u.Email == normalizedEmail))
        {
            return new ApiResponse<UserResponseDto>(HttpStatusCode.BadRequest, "Email is already taken");
        }

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

    public async Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(long userId)
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

    public async Task<ApiResponse<List<UserResponseDto>>> GetAllUsersAsync()
    {
        var foundUsers = await dbContext.Users
            .Select(u => new UserResponseDto()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
            }).ToListAsync();
        return foundUsers.Count == 0
            ? new ApiResponse<List<UserResponseDto>>(HttpStatusCode.NotFound, "Users not found")
            : new ApiResponse<List<UserResponseDto>>(HttpStatusCode.OK, foundUsers);
    }

    public async Task<ApiResponse<ConfirmationResponseDto>> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        var foundUser = await dbContext.Users.Where(u => u.Id == changePasswordDto.UserId).FirstOrDefaultAsync();
        if (foundUser == null || !foundUser.IsActive)
        {
            return new ApiResponse<ConfirmationResponseDto>(HttpStatusCode.NotFound, "User not found");
        }

        var isCurrentPassCorrect =
            BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, foundUser.Password);

        if (!isCurrentPassCorrect)
        {
            return new ApiResponse<ConfirmationResponseDto>(HttpStatusCode.BadRequest,
                "Current password is incorrect");
        }

        if (changePasswordDto.CurrentPassword == changePasswordDto.NewPassword)
        {
            return new ApiResponse<ConfirmationResponseDto>(HttpStatusCode.BadRequest,
                "Current password and new password are the same");
        }


        if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
        {
            return new ApiResponse<ConfirmationResponseDto>(HttpStatusCode.BadRequest,
                "Passwords do not match");
        }

        foundUser.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
        await dbContext.SaveChangesAsync();
        return new ApiResponse<ConfirmationResponseDto>(HttpStatusCode.NoContent, "Password changed successfully");
    }
}