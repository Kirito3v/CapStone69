public class Pagination<T>
{
    public int Pageindex { get; set; }
    public int Pagesize { get; set; }
    public int Count { get; set; }
    public List<T> Data { get; set; }

    public Pagination() { } // Required for model binding

    public Pagination(int pagesize, int pageindex, int count, List<T> data)
    {
        Pagesize = pagesize;
        Pageindex = pageindex;
        Count = count;
        Data = data;
    }
}
