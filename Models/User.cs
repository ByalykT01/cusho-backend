using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace cusho.Models;

public class User
{
    public long Id { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name cannot be longer than 50 characters")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name cannot be longer than 50 characters")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }

    public Role Role { get; set; } = Role.User;
    public DateTime Created { get; init; }
    public Address? Address { get; set; }
    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "Cart ID is required")]
    public long CartId { get; set; }

    [ForeignKey("CartId")] public Cart Cart { get; set; }
}