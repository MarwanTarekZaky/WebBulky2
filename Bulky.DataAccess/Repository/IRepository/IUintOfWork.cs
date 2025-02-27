namespace Bulky.DataAccess.Repository.IRepository;

public interface IUintOfWork
{
    ICategoryRepository CategoryRepository { get; }
    IProductRepository ProductRepository { get; }
    ICompanyRepository CompanyRepository { get; }
    IShoppingCartRepository ShoppingCartRepository { get; }
    IApplicationUserRepository ApplicationUserRepository { get; }
    void Save();
}