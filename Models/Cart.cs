namespace cusho.Models;

public class Cart
{
    public Guid Id { get; set; }
    
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
