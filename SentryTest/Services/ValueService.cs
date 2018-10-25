using System;
using System.Collections.Generic;

namespace SentryTest.Services
{
    public interface IValueService
    {
        IEnumerable<string> GetAll();
        string Get(int id);
    }

    public class ValueService : IValueService
    {
        public IEnumerable<string> GetAll()
        {
            return new[] {"first", "second"};
        }

        public string Get(int id)
        {
            throw new Exception("some exception from service to log by sentry");
        }
    }
}
