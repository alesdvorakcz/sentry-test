using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using SentryTest.Services;
using SentryTest.Swagger;
using Swashbuckle.AspNetCore.Swagger;

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
            app.UseMvc();
            ConfigureSwagger(app);
        }
        
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();

            services.AddMvcCore().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");

            services
                .AddMvc()
                .AddControllersAsServices();
            
            services.AddApiVersioning(o => o.ReportApiVersions = true);

            containerBuilder
                .RegisterType<ValueService>()
                .As<IValueService>()
                .InstancePerLifetimeScope();

            RegisterSwagger(services);

            containerBuilder.Populate(services);

            Container = containerBuilder.Build();

            return Container.Resolve<IServiceProvider>();
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        protected virtual void RegisterSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }
                c.OperationFilter<ImplicitApiVersionParameter>();

                c.OperationFilter<RemoveVersionParameters>();
                c.DocumentFilter<SetVersionInPaths>();

                c.IncludeXmlComments(XmlCommentsFilePath);
                c.CustomSchemaIds(x => x.FullName);
            });
        }

        private static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info
            {
                Title = $"Test API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "Description"
            };

            if (description.IsDeprecated)
                info.Description += " This API version has been deprecated.";

            return info;
        }

        protected virtual void ConfigureSwagger(IApplicationBuilder app)
        {
            var provider = Container.Resolve<IApiVersionDescriptionProvider>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }
    }
}
