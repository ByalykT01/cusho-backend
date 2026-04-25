namespace cusho.Models;

public class ProductImage
{
    public Guid Id { get; set; }
    
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public string Url { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
    public int SortOrder { get; set; }
}
