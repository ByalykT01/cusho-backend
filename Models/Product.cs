using System.ComponentModel.DataAnnotations;

namespace cusho.Models;

public class Product
{
    public long Id { get; set; }
    public required string Name { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [MinLength(10, ErrorMessage = "Description must be at least 10 characters.")]
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public required string ImageUrl { get; set; }
    public bool IsAvailable { get; set; }

}
