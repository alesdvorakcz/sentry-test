using System;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SentryTest.Services;

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

        public IContainer Container { get; set; }
        
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
            var containerBuilder = new ContainerBuilder();

            services
                .AddMvc()
                .AddControllersAsServices();

            containerBuilder
                .RegisterType<ValueService>()
                .As<IValueService>()
                .InstancePerLifetimeScope();

            containerBuilder.Populate(services);

            Container = containerBuilder.Build();

            return Container.Resolve<IServiceProvider>();
        }
    }
}
