using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Authentication.Infrastructure.Configuration.Swagger
{
    public class HttpMethodOrder : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {

            var orderedPaths =new  OpenApiPaths();

            foreach(var path in swaggerDoc.Paths.OrderBy(p => p.Key))
            {
                var pathItem = path.Value;
                var newPathItem = new OpenApiPathItem();
                var orderedOperations = pathItem.Operations.OrderBy(op => getOperationOrder(op.Key)).ToList();
                foreach(var (httpMethod,operation) in orderedOperations)
                {
                    newPathItem.Operations.Add(httpMethod, operation);
                }
                orderedPaths.Add(path.Key, newPathItem);

            }

            swaggerDoc.Paths = orderedPaths;
        }

        private int getOperationOrder(OperationType opType)
        {
            return opType switch
            {
                OperationType.Post => 0,
                OperationType.Get => 1,
                OperationType.Put => 2,
                OperationType.Patch => 3,
                OperationType.Delete => 4
            };
        }
    }
}
