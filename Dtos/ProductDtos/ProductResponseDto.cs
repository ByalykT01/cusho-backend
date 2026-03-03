namespace cusho.Dtos.ProductDtos;

public class ProductResponseDto
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public long? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public long? CollectionId { get; set; }
    public string? CollectionName { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ProductImageResponseDto>? Images { get; set; }
    public List<string>? Tags { get; set; }
}
