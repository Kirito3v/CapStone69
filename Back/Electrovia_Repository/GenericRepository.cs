using Electrovia_Core.Entities;
using Electrovia_Core.interfaces;
using Microsoft.EntityFrameworkCore;
using Electrovia_Core.Specifications;

namespace Electrovia_Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        #region Constructor 
        private readonly StoreDbContext _dbContext;
        public GenericRepository(StoreDbContext dbContext) // Ask CLR for creating object from DBcontext Implicitly   
        {
            _dbContext = dbContext;
        }

        #endregion

        public async Task<List<T>?> GetAllAsync()
        {
            if (typeof(T) == typeof(Products))
                return (await _dbContext.Set<Products>().Include(P => P.ProductBrand).Include(T => T.ProductType).ToListAsync()) as List<T>;
            else
                return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetbyIdAsync(int id) => await _dbContext.Set<T>().FindAsync(id);

        
        
        private IQueryable<T> ApplySpec(ISpecification<T> spec) => SpecificationEvalutor<T>.Create_Query(_dbContext.Set<T>(), spec);
        
        public async Task<List<T>> GetAllAsync(ISpecification<T> spec) => await ApplySpec(spec).ToListAsync();
        
        public async Task<T?> GetEntityAsync(ISpecification<T> spec) => await ApplySpec(spec).FirstOrDefaultAsync();
        
        public async Task<int> GetCountAsync(ISpecification<T> spec) => await ApplySpec(spec).CountAsync();
        
        

        
        public async Task AddAsync(T entity) => await _dbContext.Set<T>().AddAsync(entity);
        
        public void Update(T entity) => _dbContext.Set<T>().Update(entity);
        
        public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);

    }
}
