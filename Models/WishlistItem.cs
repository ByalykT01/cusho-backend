namespace cusho.Models;

public class WishlistItem
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public DateTime AddedAt { get; init; } = DateTime.UtcNow;
}