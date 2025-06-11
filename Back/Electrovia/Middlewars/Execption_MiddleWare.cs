using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Net;
using System.Text.Json;
using Electrovia.Errors;

namespace Electrovia.Middlewars
{
    public class ExecptionMiddleWare
    {
        #region Constructor
        private readonly RequestDelegate _next;
        private readonly ILogger<ExecptionMiddleWare> _logger;
        private readonly IHostEnvironment _environment;
        public ExecptionMiddleWare(RequestDelegate Next, ILogger<ExecptionMiddleWare> logger, IHostEnvironment environment)
        {
            _next = Next;
            _logger = logger;
            _environment = environment;
        } 
        #endregion

        public  async Task InvokeAsync(HttpContext con)
        {
            try
            {
                await _next.Invoke(con);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                con.Response.ContentType = "application/json";
                con.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _environment.IsDevelopment() ?
                    new APi_Execption_Response((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace!.ToString())
                  : new APi_Execption_Response((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace!.ToString());

                var name = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, name);
                await con.Response.WriteAsync(json);
            }
        }
    }
}