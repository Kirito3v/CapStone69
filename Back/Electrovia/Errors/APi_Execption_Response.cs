namespace Electrovia.Errors
{
    public class APi_Execption_Response : APIsResponse
    {
        public string? _Details { get; set; }
        public APi_Execption_Response(int statuscode, string? message = null, string? details= null) : base(statuscode, message)
        {
            _Details = details;
        }
    }
}
