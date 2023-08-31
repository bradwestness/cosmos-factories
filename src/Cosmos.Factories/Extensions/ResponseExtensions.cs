namespace Cosmos.Factories.Extensions;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.Cosmos;

[ExcludeFromCodeCoverage]
internal static class ResponseExtensions
{
    public static Task<ItemResponse<T>> AsTask<T>(this ItemResponse<T> value)
    {
        return Task.FromResult(value);
    }

    public static Task<FeedResponse<T>> AsTask<T>(this FeedResponse<T> value)
    {
        return Task.FromResult(value);
    }
}
