namespace cusho.Dtos.UserDtos;

public class LoginResponseDto
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string Token { get; set; }
    public string Message { get; set; } = "Login successful";
}