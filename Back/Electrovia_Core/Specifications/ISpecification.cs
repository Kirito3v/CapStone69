using System;
using System.Linq.Expressions;
using Electrovia_Core.Entities;

namespace Electrovia_Core.Specifications
{
    public interface ISpecification<T> where T : BaseEntity
    {
        // just signature for property  
        
        
        // Where 
        public Expression<Func<T, bool>> where { get; set; }
        
        // Include
        public List<Expression<Func<T,object>>> include { get; set; }

        //order by 
        public Expression<Func<T, object>> Orderby { get; set; }
        public Expression<Func<T, object>> Orderbydesc { get; set; }

        // Take & Skip
        public int _take { get; set; }
        public int _skip { get; set; }
        public bool paginationEnable  { get; set; }
    }
}
