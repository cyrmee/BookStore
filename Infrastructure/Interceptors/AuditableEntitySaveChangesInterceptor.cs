using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Infrastructure.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    static void UpdateEntities(DbContext? dbContext)
    {
        if (dbContext == null) return;

        var entries = dbContext.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .Select(e => e);

        foreach (var entry in entries)
        {
            if (entry.Entity is BaseEntity entity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.Id = Guid.NewGuid();
                        entity.CreatedDate = DateTime.UtcNow;
                        entity.UpdatedDate = DateTime.UtcNow;
                        entity.IsDeleted = false;
                        break;
                    case EntityState.Modified:
                        entity.UpdatedDate = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}