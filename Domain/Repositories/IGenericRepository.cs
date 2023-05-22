using System.Linq.Expressions;

namespace Domain.Repositories;

public interface IGenericRepository<T>
{
    public IQueryable<T> FindAll(bool trackChanges);
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);
    public void Add(T entity);
    public void AddRange(IEnumerable<T> entities);
    public void Update(T entity);
    public void Delete(T entity);
    public void DeleteRange(IEnumerable<T> entities);

    public Task<int> Count();
    public IQueryable<T> GetPaginated(int page, int pageSize);
}
