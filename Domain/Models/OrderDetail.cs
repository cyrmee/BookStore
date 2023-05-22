using Domain.Models;

namespace Domain.Models;

public class OrderDetail : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }

    public Book Book { get; set; } = null!;
    public Order Order { get; set; } = null!;
}