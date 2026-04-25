using System.ComponentModel.DataAnnotations;

namespace cusho.Dtos.ProductDtos;

public class UpdateProductDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [MinLength(10, ErrorMessage = "Description must be at least 10 characters")]
    public required string Description { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(1, double.MaxValue, ErrorMessage = "Price must be at least 1.00")]
    public decimal Price { get; set; }

    public bool IsAvailable { get; set; }

    [Range(1, long.MaxValue, ErrorMessage = "CategoryId must be greater than 0")]
    public long? CategoryId { get; set; }

    [Range(1, long.MaxValue, ErrorMessage = "CollectionId must be greater than 0")]
    public long? CollectionId { get; set; }

    public List<string>? ImageUrls { get; set; }

    public List<long>? TagIds { get; set; }
}
