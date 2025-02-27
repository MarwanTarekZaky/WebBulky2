using System.Linq.Expressions;

namespace Bulky.DataAccess.Repository.IRepository;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null, string? includeproperties = null);
    T Get(Expression<Func<T, bool>> predicate, string? includeproperties = null, bool tracked = false);
    void Add(T entity);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
}