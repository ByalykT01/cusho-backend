namespace cusho.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public Guid CollectionId { get; set; }
    public Collection? Collection { get; set; }
    
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
}
