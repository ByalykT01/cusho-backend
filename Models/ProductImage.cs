using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cusho.Models;

public class ProductImage
{
    public long Id { get; set; }

    [Required]
    public long ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }

    [Required(ErrorMessage = "Image URL is required")]
    public required string Url { get; set; }

    public bool IsPrimary { get; set; }

    public int SortOrder { get; set; }
}
