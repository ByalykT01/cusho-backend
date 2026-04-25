namespace cusho.Models;

public class CartItem
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public Guid CartId { get; set; }
    public Cart Cart { get; set; } = null!;

    public int Count { get; set; } = 1;
}
