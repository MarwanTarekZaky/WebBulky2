using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;

namespace Bulky.DataAccess.Repository;

public class UintOfWork: IUintOfWork
{
    public ICategoryRepository CategoryRepository { get; private set; }
    public IProductRepository ProductRepository { get; private set; }
    public ICompanyRepository CompanyRepository { get; private set; }
    public IShoppingCartRepository ShoppingCartRepository { get; private set; }
    public IApplicationUserRepository ApplicationUserRepository { get; private set; }
    public IOrderHeaderRepository OrderHeader { get; private set; }
    public IOrderDetailRepository OrderDetail { get; private set; }
    private ApplicationDbContext _db;
    
    public UintOfWork(ApplicationDbContext db) 
    {
        _db = db;
        CategoryRepository = new CategoryRepository(_db);
        ProductRepository = new ProductRepository(_db);
        CompanyRepository = new CompanyRepository(_db);
        ShoppingCartRepository = new ShoppingCartRepository(_db);
        ApplicationUserRepository = new ApplicationUserRepository(_db);
        OrderHeader = new OrderHeaderRepository(_db);
        OrderDetail = new OrderDetailRepository(_db);   
        
    }
    public void Save()
    {
        _db.SaveChanges();
    }
}