namespace cusho.Models;

public class Order
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
    
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
