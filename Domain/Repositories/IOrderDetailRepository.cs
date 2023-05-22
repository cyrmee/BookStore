using Domain.Models;

namespace Domain.Repositories;

public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
{
    public Task<List<OrderDetail>> GetAllAsync();
}