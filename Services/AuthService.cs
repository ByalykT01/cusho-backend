using cusho.Data;
using cusho.Dtos.UserDtos;

namespace cusho.Services;

public class AuthService(ApplicationDbContext dbContext)
{
    public bool ValidateUser(LoginDto loginDto)
    {
        var foundUser = dbContext.Users.FirstOrDefault(u => u.Email == loginDto.Email);
        if (foundUser is null) return false;
        return BCrypt.Net.BCrypt.Verify(loginDto.Password, foundUser.Password);
    }
}