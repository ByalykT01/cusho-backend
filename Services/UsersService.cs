using System.Net;
using cusho.Data;
using cusho.Dtos;
using cusho.Dtos.UserDtos;
using Microsoft.EntityFrameworkCore;

namespace cusho.Services;

public class UsersService(ApplicationDbContext dbContext, ILogger<UsersService> logger)
{

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

    public async Task<ApiResponse<ConfirmationResponseDto>> ChangePasswordAsync(string email, ChangePasswordDto changePasswordDto)
    {
        var foundUser = await dbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
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
