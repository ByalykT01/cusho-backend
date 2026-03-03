using System.ComponentModel.DataAnnotations;

namespace cusho.Models;

public class Category
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public ICollection<Product>? Products { get; set; }
}
