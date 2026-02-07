using System.ComponentModel.DataAnnotations.Schema;

namespace cusho.Models;

public class CartItem
{
    public long Id { get; set; }
    public required Product Product { get; set; }
    
    [ForeignKey("Product")] 
    public long ProductId { get; set; }

    public int Count { get; set; } = 1;
}