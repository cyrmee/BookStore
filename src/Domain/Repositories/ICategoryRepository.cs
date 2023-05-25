using Domain.Models;

namespace Domain.Repositories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    public Task<List<Category>> GetAllAsync();
}