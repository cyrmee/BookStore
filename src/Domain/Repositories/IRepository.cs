namespace Domain.Repositories;

public interface IRepository
{
    public IBookRepository? Book { get; }
    public IBookCategoryRepository? BookCategory { get; }
    public ICategoryRepository? Category { get; }
    public IOrderRepository? Order { get; }
    public IOrderDetailRepository? OrderDetail { get; }
    public IJwtTokensRepository? JwtTokens { get; }
    public Task SaveAsync();
}
