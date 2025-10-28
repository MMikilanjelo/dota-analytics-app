
namespace SharedKernel.Contracts.Data;

public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}