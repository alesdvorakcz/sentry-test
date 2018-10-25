using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SentryTest.Services;

namespace SentryTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : BaseController
    {
        private readonly IValueService _valueService;

        public ValuesController(IValueService valueService, ILogger<ValuesController> logger) : base(logger)
        {
            _valueService = valueService;
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return HandleExceptions(() =>
            {
                var result = _valueService.GetAll();
                return Ok(result);
            });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id == 4)
                throw new Exception("Exception from controller");

            return HandleExceptions(() =>
            {
                var result = _valueService.Get(id);
                return Ok(result);
            });
        }
    }
}
