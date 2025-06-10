namespace Electrovia.Errors
{
    public class API_ValidationError_Response : APIsResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public API_ValidationError_Response(): base(400)
        {
            Errors = new List<string>();
        }
    }
}
