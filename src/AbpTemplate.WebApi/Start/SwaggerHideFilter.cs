using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AbpTemplate.WebApi.Start
{
    public class SwaggerHideFilter : IDocumentFilter
    {
        private readonly string[] _apiPathesToHide = new[]
        {
            "/api/abp/application-configuration",
            "/api/abp/api-definition"
        };

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths.RemoveAll(ShouldBeHidden);
        }

        private bool ShouldBeHidden(KeyValuePair<string, OpenApiPathItem> pathKV)
        {
            return _apiPathesToHide.Contains(pathKV.Key);
        }
    }
}