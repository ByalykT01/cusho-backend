namespace cusho.Dtos.ProductDtos;

public class ProductImageResponseDto
{
    public required long Id { get; set; }
    public required string Url { get; set; }
    public bool IsPrimary { get; set; }
    public int SortOrder { get; set; }
}
