using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess;

public class BookRepository : GenericRepository<Book>, IBookRepository
{
    public BookRepository(BookStoreDbContext context) : base(context)
    {
    }

    public async Task<List<Book>> GetAllAsync()
        => await FindAll(false).ToListAsync();
}
