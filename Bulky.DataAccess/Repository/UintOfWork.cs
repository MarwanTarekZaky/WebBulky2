using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;

namespace Bulky.DataAccess.Repository;

public class UintOfWork: IUintOfWork
{
    public ICategoryRepository CategoryRepository { get; private set; }
    private ApplicationDbContext _db;
    
    public UintOfWork(ApplicationDbContext db) 
    {
        _db = db;
        CategoryRepository = new CategoryRepository(_db);
    }
    public void Save()
    {
        _db.SaveChanges();
    }
}