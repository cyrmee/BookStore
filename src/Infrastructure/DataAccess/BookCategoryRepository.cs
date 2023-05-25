using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess;

public class BookCategoryRepository : GenericRepository<BookCategory>, IBookCategoryRepository
{
    public BookCategoryRepository(BookStoreDbContext context) : base(context)
    {
    }

    public async Task<List<BookCategory>> GetAllAsync()
        => await FindAll(false).ToListAsync();
}