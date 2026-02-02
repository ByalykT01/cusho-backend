namespace cusho.Models;

public class User
{
    public long Id { get; set; }
    public required string Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public bool IsActive { get; set; }
    public required long CartId { get; set; }

}
