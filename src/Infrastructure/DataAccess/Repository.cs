using Domain.Repositories;

namespace Infrastructure.DataAccess;

public class Repository : IRepository
{
    public Repository(BookStoreDbContext context)
        => _context = context;

    private readonly BookStoreDbContext _context;

    private IBookRepository? BookRepository { get; set; }
    private IBookCategoryRepository? BookCategoryRepository { get; set; }
    private ICategoryRepository? CategoryRepository { get; set; }
    private IOrderRepository? OrderRepository { get; set; }
    private IOrderDetailRepository? OrderDetailRepository { get; set; }
    private IJwtTokensRepository? JwtTokensRepository { get; set; }

    public IBookRepository? Book
    {
        get
        {
            BookRepository ??= new BookRepository(_context);
            return BookRepository;
        }
    }

    public IBookCategoryRepository? BookCategory
    {
        get
        {
            BookCategoryRepository ??= new BookCategoryRepository(_context);
            return BookCategoryRepository;
        }
    }

    public ICategoryRepository? Category
    {
        get
        {
            CategoryRepository ??= new CategoryRepository(_context);
            return CategoryRepository;
        }
    }

    public IOrderRepository? Order
    {
        get
        {
            OrderRepository ??= new OrderRepository(_context);
            return OrderRepository;
        }
    }

    public IOrderDetailRepository? OrderDetail
    {
        get
        {
            OrderDetailRepository ??= new OrderDetailRepository(_context);
            return OrderDetailRepository;
        }
    }

    public IJwtTokensRepository? JwtTokens
    {
        get
        {
            JwtTokensRepository ??= new JwtTokensRepository(_context);
            return JwtTokensRepository;
        }
    }
    public async Task SaveAsync()
        => await _context.SaveChangesAsync();
}

