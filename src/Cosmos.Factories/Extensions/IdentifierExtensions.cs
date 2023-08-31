namespace Cosmos.Factories.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

[ExcludeFromCodeCoverage]
internal static class IdentifierExtensions
{
    public static string GetIdentifier(this object value, string propertyName = "id")
    {
        if (value is not null)
        {
            if (value.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase) is PropertyInfo property
                && property.GetValue(value) is string propertyValue
                && !string.IsNullOrWhiteSpace(propertyValue))
            {
                return propertyValue;
            }

            if (value.GetType().GetField(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase) is FieldInfo field
                && field.GetValue(value) is string fieldValue
                && !string.IsNullOrWhiteSpace(fieldValue))
            {
                return fieldValue;
            }
        }

        return Guid.NewGuid().ToString();
    }
}
