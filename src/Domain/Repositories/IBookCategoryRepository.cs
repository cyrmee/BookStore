using Domain.Models;

namespace Domain.Repositories;

public interface IBookCategoryRepository : IGenericRepository<BookCategory>
{
    public Task<List<BookCategory>> GetAllAsync();
}