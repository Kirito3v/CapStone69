namespace Electrovia_Core.Specifications
{
    public class Product_Spec_Parameters
    {
        public int? brandid { get; set; }
        public int? typeid { get; set; }
        public string? sort { get; set; }

        private string? search;
        public string? Search
        {
            get { return search!; }
            set { search = value?.ToLower(); }
        }

        public int Pageindex { get; set; } = 1;
        private const int max_page_size = 20;
        private int pagesize = 20;
        public int Pagesize
        {
            get { return pagesize; }
            set { pagesize = value > max_page_size ? max_page_size : value ; }
        }
    }
}
