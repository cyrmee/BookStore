using Domain.Types;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Order : BaseEntity
{
    public string UserName { get; set; } = null!;

    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    public DateTime OrderDate { get; set; }
    public double TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.New;

    public User User { get; set; } = null!;
    public OrderDetail? OrderDetail { get; set; }
}
