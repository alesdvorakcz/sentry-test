using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SentryTest
{
    public class Startup : IStartup
    {
        public Startup(IHostingEnvironment env, IConfiguration conf)
        {
            Environment = env;
            Configuration = conf;
        }

        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
        
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            return services.BuildServiceProvider();
        }
    }
}
