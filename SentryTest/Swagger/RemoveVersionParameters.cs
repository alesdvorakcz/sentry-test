using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SentryTest.Swagger
{
    public class RemoveVersionParameters : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            // Remove version parameter from all Operations
            var versionParameter = operation.Parameters.Single(p => p.Name == "api-version");
            operation.Parameters.Remove(versionParameter);
        }
    }
}