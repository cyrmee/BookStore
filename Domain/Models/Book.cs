using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Book : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Publisher { get; set; } = null!;

    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    public DateTime PublicationDate { get; set; }
    public string ISBN { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Quantity { get; set; } = 0;
    public double Price { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
    public ICollection<BookCategory> BookCategories { get; } = new List<BookCategory>();
}
