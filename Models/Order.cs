using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cusho.Models;

public class Order
{
    public long Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public ICollection<OrderItem>? Items { get; set; }
}
