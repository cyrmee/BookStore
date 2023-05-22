using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess;

public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
{
    public OrderDetailRepository(BookStoreDbContext context) : base(context)
    {
    }

    public async Task<List<OrderDetail>> GetAllAsync()
        => await FindAll(false).ToListAsync();
}