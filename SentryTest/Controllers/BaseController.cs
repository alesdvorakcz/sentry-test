using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SentryTest.Controllers
{
    public abstract class BaseController : Controller
    {
        protected ILogger Logger { get; }

        protected BaseController(ILogger logger)
        {
            Logger = logger;
        }

        protected IActionResult HandleExceptions(Func<IActionResult> action)
        {
            try
            {
                return action();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return new JsonResult(new { Error = e.Message })
                {
                    StatusCode = 500,
                };
            }
        }
    }
}