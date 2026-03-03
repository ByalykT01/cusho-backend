using System.ComponentModel.DataAnnotations;

namespace cusho.Models;

public class Tag
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Tag name is required")]
    [StringLength(30, MinimumLength = 1, ErrorMessage = "Tag name must be between 1 and 30 characters")]
    public required string Name { get; set; }

    public ICollection<ProductTag>? ProductTags { get; set; }
}
