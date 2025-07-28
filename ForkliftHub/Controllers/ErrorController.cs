using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ForkliftHub.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]

    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult NotFoundPage()
        {
            var vm = new ErrorViewModel
            {
                StatusCode = 404,
                Message = "The page you are looking for could not be found.",
                RequestId = HttpContext.TraceIdentifier
            };
            return View("NotFound", vm);
        }

        [Route("Error/500")]
        public IActionResult ServerError()
        {
            var vm = new ErrorViewModel
            {
                StatusCode = 500,
                Message = "An unexpected error occurred.",
                RequestId = HttpContext.TraceIdentifier
            };
            return View("ServerError", vm);
        }
    }
}
