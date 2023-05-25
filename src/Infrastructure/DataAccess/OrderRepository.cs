using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(BookStoreDbContext context) : base(context)
    {
    }

    public async Task<List<Order>> GetAllAsync()
        => await FindAll(false).ToListAsync();
}