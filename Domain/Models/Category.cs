namespace Domain.Models;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;

    public IEnumerable<BookCategory> BookCategories { get; } = new List<BookCategory>();
}