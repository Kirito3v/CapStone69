namespace Electrovia.Errors
{
    public class APIsResponse
    {
        public int _StatusCode { get; set; }
        public string? _Message { get; set; }
        public APIsResponse(int statusCode, string? message = null)
        {
            _StatusCode = statusCode;
            _Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "BadRequest, you have made ",
                401 => "UnAuthorized , you are not",
                404 => "Resource was Not Found",
                500 => "Errors are the path to the dark side. Errors lead to anger. Anger lead to hate. Hate leads to Career change",
                _ => null
            };
        }
    }
}