using Electrovia.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Electrovia.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            return NotFound(new APIsResponse(code));
        }
    }
}
