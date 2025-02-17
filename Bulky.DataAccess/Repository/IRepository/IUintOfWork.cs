namespace Bulky.DataAccess.Repository.IRepository;

public interface IUintOfWork
{
    ICategoryRepository CategoryRepository { get; }
    void Save();
}