using System.Collections.Generic;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace ModernizationDemo.WebApi.OpenApi
{
    public class AcceptFileOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            operation.consumes = new List<string>() { "multipart/form-data" };
            operation.parameters.Add(new Parameter()
            {
                name = "file",
                required = true,
                type = "file",
                @in = "formData"
            });
        }
    }
}