namespace Domain.Models;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;

    public ICollection<BookCategory> BookCategories { get; } = new List<BookCategory>();
}