using System.Linq.Expressions;
using Electrovia_Core.Entities;
using Electrovia_Core.Specifications;

namespace Electrovia_Repository
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        //Automatic property -> compiler will generate backing field hidden private attribute
        public Expression<Func<T, bool>> where { get; set; }
        public List<Expression<Func<T, object>>> include { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> Orderby { get; set; }
        public Expression<Func<T, object>> Orderbydesc { get; set; }
        public int _take { get ; set; }
        public int _skip { get; set; }
        public bool paginationEnable { get; set; }

        public BaseSpecification(){}
        public BaseSpecification(Expression<Func<T, bool>> where_expression)
        {
            this.where = where_expression;
        }
        public void Add_OrederBy(Expression<Func<T, object>> Orederby) => Orderby = Orederby;
        public void Add_OrederByDesc(Expression<Func<T, object>> Orederbydesc) => Orderbydesc = Orederbydesc;
        public void Applypagination(int skip, int take)
        {
            paginationEnable = true;
            _take = take;
            _skip = skip;
        }
    }
}
