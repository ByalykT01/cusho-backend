using System.ComponentModel.DataAnnotations;

namespace cusho.Dtos.UserDtos;

public class UserUpdateDto
{
    [Required(ErrorMessage = "UserId is required")]
    public long UserId { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    [MinLength(2, ErrorMessage = "First Name must be at least 2 characters")]
    [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [MinLength(2, ErrorMessage = "Last Name must be at least 2 characters")]
    [MaxLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
}