using Electrovia_Core.Entities;
using Electrovia_Core.Specifications;

namespace Electrovia_Core.interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetbyIdAsync(int id);


        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task<IReadOnlyList<T>> GetAllAsync(ISpecification<T> spec);
        Task<T> GetEntityAsync(ISpecification<T> spec);
        Task<int> GetCountAsync(ISpecification<T> spec);
    }
}
