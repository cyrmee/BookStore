using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess;

public class JwtTokensRepository : GenericRepository<JwtTokens>, IJwtTokensRepository
{
    public JwtTokensRepository(BookStoreDbContext context) : base(context)
    {
    }

    public async Task<List<JwtTokens>> GetAllAsync()
        => await FindAll(false).ToListAsync();
}