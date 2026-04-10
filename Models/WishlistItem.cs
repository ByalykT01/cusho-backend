using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cusho.Models;

public class WishlistItem
{
    public long Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    [Required]
    public long ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }

    public DateTime AddedAt { get; init; } = DateTime.UtcNow;
}
