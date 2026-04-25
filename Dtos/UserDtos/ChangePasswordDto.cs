using System.ComponentModel.DataAnnotations;

namespace cusho.Dtos.UserDtos;

public class ChangePasswordDto
{
    [Required(ErrorMessage = "Current Password is required")]
    [DataType(DataType.Password)]
    public required string CurrentPassword { get; set; }

    [Required(ErrorMessage = "New Password is required")]
    [MinLength(8, ErrorMessage = "New Password must be at least 8 characters")]
    [DataType(DataType.Password)]
    public required string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm New Password is required")]
    [Compare("NewPassword", ErrorMessage = "New Password and Confirm New Password do not match")]
    [DataType(DataType.Password)]
    public required string ConfirmNewPassword { get; set; }
}
