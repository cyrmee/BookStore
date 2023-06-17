using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Jobs;

public interface ITokenCleanupJob
{
    Task RemoveRevokedTokens();
}

public class TokenCleanupJob : ITokenCleanupJob
{
    private readonly IRepository _repository;

    public TokenCleanupJob(IRepository repository)
    {
        _repository = repository;
    }

    public async Task RemoveRevokedTokens()
    {
        var expiredTokens = await _repository.JwtTokens!
                .FindByCondition(t => t.ExpirationDate < DateTime.UtcNow || t.IsRevoked)
                .ToListAsync();

        _repository.JwtTokens.DeleteRange(expiredTokens);
        await _repository.SaveAsync();
    }
}
