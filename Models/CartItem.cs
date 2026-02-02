namespace cusho.Models;

public class CartItem
{
    public long Id { get; set; }
    public required Product Product { get; set; }
    public int Count { get; set;  }
}
