namespace Cosmos.Factories.Extensions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal static class EnumerableExtensions
{
    public static IEnumerable<TOutput> ToTyped<TInput, TOutput>(this IEnumerable<TInput> values)
    {
        if (values is null)
        {
            yield break;
        }

        foreach (var value in values)
        {
            if (value is TOutput item)
            {
                yield return item;
            }
        }
    }

    public static IEnumerable<T> ToTyped<T>(this IEnumerable<object> values)
    {
        if (values is null)
        {
            yield break;
        }

        foreach (var value in values)
        {
            if (value is T item)
            {
                yield return item;
            }
        }
    }
}
