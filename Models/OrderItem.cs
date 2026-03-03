using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cusho.Models;

public class OrderItem
{
    public long Id { get; set; }

    [Required]
    public long OrderId { get; set; }

    [ForeignKey("OrderId")]
    public Order? Order { get; set; }

    [Required]
    public long ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal PriceAtPurchase { get; set; }
}
