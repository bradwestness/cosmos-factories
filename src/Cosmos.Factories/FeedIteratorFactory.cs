namespace Cosmos.Factories;

using Cosmos.Factories.Extensions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

/// <summary>
/// This is the default implementation of IFeedIteratorFactory which should be
/// injected in a non-test context (by registering it with the
/// <see cref="ServiceCollectionExtensions.AddFeedIteratorFactory(Microsoft.Extensions.DependencyInjection.IServiceCollection)"/> extension method).
/// 
/// The implementation of thee <see cref="GetFeedIterator{T}(IQueryable{T})"/> method calls the Cosmos client
/// library's <see cref="CosmosLinqExtensions.ToFeedIterator{T}(IQueryable{T})"/> extension method, which throws
/// an <see cref="ArgumentOutOfRangeException"/> if the IQueryable is not an instance of
/// the internal, sealed CosmosLinqQuery&lt;T&gt; type.
/// </summary>
public sealed class FeedIteratorFactory : IFeedIteratorFactory
{
    public FeedIterator<T> GetFeedIterator<T>(IQueryable<T> query)
    {
        return query.ToFeedIterator();
    }
}
