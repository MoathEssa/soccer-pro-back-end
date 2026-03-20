using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace SoccerPro.API.Controllers.settings;
public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum) return;

        var enumDescriptions = Enum.GetNames(context.Type)
            .Select(name =>
            {
                var value = ((int)Enum.Parse(context.Type, name));
                return $"{value} = {name}";
            });

        schema.Description += string.Join(", ", enumDescriptions);
    }
}
