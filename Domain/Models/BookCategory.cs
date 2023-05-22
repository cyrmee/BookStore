namespace Domain.Models;

public class BookCategory : BaseEntity
{
    public Guid BookId { get; set; }
    public Guid CategoryId { get; set; }

    public Book Book { get; set; } = null!;
    public Category Category { get; set; } = null!;
}
