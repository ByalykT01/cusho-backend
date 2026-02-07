using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cusho.Models;

public class Cart
{
    public long Id { get; set; }

    public ICollection<CartItem>? CartItems { get; set; }
}