using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace SentryTest.Services
{
    public interface IValueService
    {
        IEnumerable<string> GetAll();
        string Get(int id);
    }

    public class ValueService : IValueService
    {
        private readonly ILogger<ValueService> _logger;

        public ValueService(ILogger<ValueService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<string> GetAll()
        {
            _logger.LogInformation("GetAll was called");
            return new[] {"first", "second"};
        }

        public string Get(int id)
        {
            _logger.LogInformation($"Get {id} was called");
            throw new Exception("some exception from service to log by sentry");
        }
    }
}
