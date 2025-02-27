using System.Linq.Expressions;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Repository;
using Bulky.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository;

public class Repository<T>: IRepository<T> where T: class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> _dbSet;
    public Repository(ApplicationDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<T>();
        _db.Products.Include(e => e.Category).Include(e => e.CategoryId);
    }
    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null, string? includeproperties = null)
    {
        IQueryable<T> query = _dbSet;
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
     
        if (!string.IsNullOrWhiteSpace(includeproperties))
        {
            foreach (var property in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }
        return query.ToList();
    }

    public T Get(Expression<Func<T, bool>> filter, string? includeproperties = null, bool tracked = false)
    {
        
        IQueryable<T> query;
        if (tracked)
        {
            query = _dbSet;
        }
        else
        {
            query = _dbSet.AsNoTracking();
        }
        query = query.Where(filter);
        if (!string.IsNullOrWhiteSpace(includeproperties))
        {
            foreach (var property in includeproperties
                         .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }
        return query.FirstOrDefault();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}