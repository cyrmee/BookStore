using Domain.Models;

namespace Domain.Repositories;

public interface IJwtTokensRepository : IGenericRepository<JwtTokens>
{
     public Task<List<JwtTokens>> GetAllAsync();
}