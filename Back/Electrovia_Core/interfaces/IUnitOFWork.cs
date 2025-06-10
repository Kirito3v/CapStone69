using Electrovia_Core.Entities;
using Electrovia_Core.Entities.Order_Aggregate;

namespace Electrovia_Core.interfaces
{
    public interface IUnitOFWork : IAsyncDisposable 
    {
        IGenericRepository<T>? Repository<T>() where T : BaseEntity;
        Task<int> Complete();
    }
}
