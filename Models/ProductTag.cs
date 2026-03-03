using System.ComponentModel.DataAnnotations.Schema;

namespace cusho.Models;

public class ProductTag
{
    public long ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }

    public long TagId { get; set; }

    [ForeignKey("TagId")]
    public Tag? Tag { get; set; }
}
