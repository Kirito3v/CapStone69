using Electrovia_Core.Entities;
using Microsoft.EntityFrameworkCore;
using Electrovia_Core.Specifications;

namespace Electrovia_Repository
{
    public static class SpecificationEvalutor<T> where T : BaseEntity
    {
        public static IQueryable<T> Create_Query(IQueryable<T> input_Query, ISpecification<T> spec)
        {
            var query = input_Query; // Query = _dbcontext.set<Products>()

            if (spec.where is not null)  // Query =  _dbcontext.Set<Products>().where(P => P.Id == id)
                query = query.Where(spec.where);
            
            if (spec.Orderby is not null)
                query = query.OrderBy(spec.Orderby);

            if (spec.Orderbydesc is not null)
                query = query.OrderByDescending(spec.Orderbydesc);
            
            if (spec.paginationEnable is true)
                query = query.Skip(spec._skip).Take(spec._take);

            query = spec.include.Aggregate(query , (CurrentQuery, include_expression ) => CurrentQuery.Include(include_expression));

            return query;
        }
    }
}   