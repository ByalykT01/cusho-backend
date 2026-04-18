using cusho.Data;
using cusho.Dtos;
using cusho.Dtos.UserDtos;
using cusho.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace cusho.Services;

public class UsersService(ApplicationDbContext dbContext, ILogger<UsersService> logger)
{

    public async Task<Result<UserResponseDto>> GetUserByIdAsync(Guid userId)
    {
        

        var foundUser = await dbContext.Users.AsNoTracking().Where(u => u.Id.Equals(userId))
            .Select(u => new UserResponseDto()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
            })
            .FirstOrDefaultAsync();

        return foundUser == null
            ? Result<UserResponseDto>.Failure("User not found")
            : Result<UserResponseDto>.Success(foundUser);
    }

    public async Task<Result<List<UserResponseDto>>> GetAllUsersAsync()
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
            ? Result<List<UserResponseDto>>.Failure("Users not found")
            : Result<List<UserResponseDto>>.Success(foundUsers);
    }

    public async Task<Result<ConfirmationResponseDto>> ChangePasswordAsync(string email, ChangePasswordDto changePasswordDto)
    {

        var foundUser = await dbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        if (foundUser == null || !foundUser.IsActive)
        {
            logger.LogWarning("Password change failed because user is missing or inactive.");
            return Result<ConfirmationResponseDto>.Failure("User not found");
        }

        var isCurrentPassCorrect =
            BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, foundUser.Password);

        if (!isCurrentPassCorrect)
        {
            logger.LogWarning("Password change failed due to invalid current password for user {UserId}.", foundUser.Id);
            return Result<ConfirmationResponseDto>.Failure("Current password is incorrect");
        }

        if (changePasswordDto.CurrentPassword == changePasswordDto.NewPassword)
        {
            logger.LogWarning("Password change failed because new password equals current password for user {UserId}.", foundUser.Id);
            return Result<ConfirmationResponseDto>.Failure("Current password and new password are the same");
        }


        if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
        {
            logger.LogWarning("Password change failed because password confirmation did not match for user {UserId}.", foundUser.Id);
            return Result<ConfirmationResponseDto>.Failure("Passwords do not match");
        }

        foundUser.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Password changed successfully for user {UserId}.", foundUser.Id);
        return Result<ConfirmationResponseDto>.Success(new() { Message = "Password changed successfully" });
    }
}
