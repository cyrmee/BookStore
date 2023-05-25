using Domain.Models;

namespace Domain.Repositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    public Task<List<Order>> GetAllAsync();
}