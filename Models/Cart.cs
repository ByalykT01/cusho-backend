namespace cusho.Models;

public class Cart {
    public long Id { get; set; }
    public required User User { get; set; } = null!;
    public ICollection<CartItem>? CartItems { get; set; }
}
