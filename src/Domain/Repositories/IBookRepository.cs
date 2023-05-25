using Domain.Models;

namespace Domain.Repositories;

public interface IBookRepository : IGenericRepository<Book>
{
    public Task<List<Book>> GetAllAsync();
}