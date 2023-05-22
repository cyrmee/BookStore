using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.DataAccess;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    public GenericRepository(BookStoreDbContext context)
        => _context = context;

    private readonly BookStoreDbContext _context;

    public IQueryable<T> FindAll(bool trackChanges)
        =>
            !trackChanges
                ? _context.Set<T>()
                    .AsNoTracking()
                : _context.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
        => !trackChanges
                ? _context.Set<T>()
                    .Where(expression)
                    .AsNoTracking()
                : _context.Set<T>()
                    .Where(expression);

    public void Add(T entity)
        => _context.Set<T>().Add(entity);

    public void AddRange(IEnumerable<T> entities)
        => _context.Set<T>().AddRange(entities);

    public void Update(T entity)
        => _context.Set<T>().Update(entity);

    public void Delete(T entity)
        => _context.Set<T>().Remove(entity);

    public void DeleteRange(IEnumerable<T> entities)
        => _context.Set<T>().RemoveRange(entities);

    public async Task<int> Count()
        => await _context.Set<T>().CountAsync();

    public IQueryable<T> GetPaginated(int page, int pageSize)
    => _context.Set<T>()
        .OrderBy(m => m.CreatedDate)
        .ThenBy(m => m.UpdatedDate)
        .Skip((page - 1) * pageSize)
        .Take(pageSize);

}
