using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SentryTest.Services;

namespace SentryTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IValueService _valueService;

        public ValuesController(IValueService valueService)
        {
            _valueService = valueService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(_valueService.GetAll());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return Ok(_valueService.Get(id));
        }
    }
}
