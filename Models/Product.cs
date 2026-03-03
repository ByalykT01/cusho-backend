using System.ComponentModel.DataAnnotations;

namespace cusho.Models;

public class Product
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name cannot be longer than 50 characters")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [MinLength(10, ErrorMessage = "Description must be at least 10 characters.")]
    public required string Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(1, double.MaxValue, ErrorMessage = "Price must be at least 1.00.")]
    public decimal Price { get; set; }

    public bool IsAvailable { get; set; }

    public long? CategoryId { get; set; }
    public Category? Category { get; set; }

    public long? CollectionId { get; set; }
    public Collection? Collection { get; set; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public ICollection<ProductImage>? Images { get; set; }
    public ICollection<ProductTag>? ProductTags { get; set; }
}
