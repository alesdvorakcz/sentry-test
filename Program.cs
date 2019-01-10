using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SentryTest
{
    [ApiController, Route("/")]
    public class Program
    {
        [HttpGet]
        public IActionResult Get() => throw new InvalidOperationException();

        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .Configure(a =>
                {
                    a.UseMvc();
                    a.UseSwagger();
                })
                .ConfigureServices(s =>
                {
                    s.AddMvc();

                    s.AddSwaggerGen(c =>
                    {
                        var provider = s.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
                    });
                })
                .UseSentry(o =>
                {
                    o.Dsn = "https://5fd7a6cda8444965bade9ccfd3df9882@sentry.io/1188141";
                    o.Debug = true;
                });
    }
}
