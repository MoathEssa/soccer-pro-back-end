using FluentValidation;
using FluentValidation.Validators;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
namespace SoccerPro.API.Controllers.settings;
public class FluentValidationSchemaFilter : ISchemaFilter
{
    private readonly IServiceScopeFactory _scopeFactory;

    public FluentValidationSchemaFilter(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        using var scope = _scopeFactory.CreateScope();

        if (context.Type.IsPrimitive || context.Type == typeof(string))
            return;

        var validatorType = typeof(IValidator<>).MakeGenericType(context.Type);
        var validator = scope.ServiceProvider.GetService(validatorType) as IValidator;

        if (validator == null)
            return;

        var descriptor = validator.CreateDescriptor();

        foreach (var property in schema.Properties)
        {
            var jsonPropertyName = property.Key;

            // Match actual C# property name (case-insensitive)
            var actualProperty = context.Type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => 
                    string.Equals(p.Name, jsonPropertyName, StringComparison.OrdinalIgnoreCase));

            if (actualProperty == null)
                continue;

            var propertyName = actualProperty.Name;

            foreach (var (validatorRule, _) in descriptor.GetValidatorsForMember(propertyName))
            {
                // Required fields
                if (validatorRule is INotNullValidator or INotEmptyValidator)
                {
                    if (!schema.Required.Contains(jsonPropertyName))
                        schema.Required.Add(jsonPropertyName);
                }

                // String length
                if (validatorRule is ILengthValidator length)
                {
                    if (length.Max > 0)
                        property.Value.MaxLength = length.Max;

                    if (length.Min > 0)
                        property.Value.MinLength = length.Min;
                }

                // Regex pattern
                if (validatorRule is IRegularExpressionValidator regex)
                {
                    property.Value.Pattern = regex.Expression;
                }

                // Email format
                if (validatorRule is IEmailValidator)
                {
                    property.Value.Format = "email";
                }

                // Number min/max
                if (validatorRule is IComparisonValidator cmp)
                {
                    if (decimal.TryParse(cmp.ValueToCompare?.ToString(), out var value))
                    {
                        switch (cmp.Comparison)
                        {
                            case Comparison.GreaterThan:
                            case Comparison.GreaterThanOrEqual:
                                property.Value.Minimum = value;
                                break;

                            case Comparison.LessThan:
                            case Comparison.LessThanOrEqual:
                                property.Value.Maximum = value;
                                break;
                        }
                    }
                }

                // Collection minItems
                if ((validatorRule is INotEmptyValidator or INotNullValidator) &&
                    actualProperty.PropertyType != typeof(string) &&
                    typeof(System.Collections.IEnumerable).IsAssignableFrom(actualProperty.PropertyType))
                {
                    property.Value.MinItems = 1;
                }
            }
        }
    }
}
