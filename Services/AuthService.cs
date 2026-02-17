using cusho.Data;
using cusho.Dtos.UserDtos;

namespace cusho.Services;

public class AuthService(ApplicationDbContext dbContext)
{
    public Boolean ValidateUser(LoginDto loginDto)
    {
        var foundUser = dbContext.Users.FirstOrDefault(u => u.Email == loginDto.Email);
        return BCrypt.Net.BCrypt.Verify(loginDto.Password, foundUser?.Password);
    }
}