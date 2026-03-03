using System.ComponentModel.DataAnnotations;

namespace cusho.Models;

public class Collection
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public ICollection<Product>? Products { get; set; }
}
