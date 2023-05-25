using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(BookStoreDbContext context) : base(context)
    {
    }

    public async Task<List<Category>> GetAllAsync()
        => await FindAll(false).ToListAsync();
}