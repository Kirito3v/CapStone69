using Electrovia.DTOs;

namespace Electrovia.Helpers
{
    public class Pagination<T>
    {
        public int Pageindex { get; set; }
        public int Pagesize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
        public Pagination(int pagesize, int pageindex, int count, IReadOnlyList<T> data)
        {
            Pagesize = pagesize;
            Pageindex = pageindex;
            Count = count;
            Data = data;
        }
    }
}
