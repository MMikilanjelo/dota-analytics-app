
namespace Common.Contracts;

public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}