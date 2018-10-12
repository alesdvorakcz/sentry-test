using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SentryTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .SetBasePath(context.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IStartup, Startup>();
                })
                .UseSentry();
    }
}
