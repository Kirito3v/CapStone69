using Electrovia_Core.Entities;
using Electrovia_Core.interfaces;
using System.Collections;


namespace Electrovia_Repository
{
    public class UnitOfWork : IUnitOFWork
    {
        #region Constructor 
        private readonly StoreDbContext _context;
        private Hashtable _Repository;
        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
        } 

        #endregion 
        public IGenericRepository<T>? Repository<T>() where T : BaseEntity
        {
            if (_Repository == null) _Repository = new Hashtable(); 
         
            var type  = typeof(T).Name;
            
            if (!_Repository.ContainsKey(type))
            {
                var repo = new GenericRepository<T>(_context); 
                _Repository.Add(type, repo);
            }
            return _Repository[type] as IGenericRepository<T>;
        }
        public async Task<int> Complete() => await _context.SaveChangesAsync();
        public async ValueTask DisposeAsync() =>await _context.DisposeAsync();
    }
}
