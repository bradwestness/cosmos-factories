namespace Cosmos.Factories.Fakes;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.Cosmos;

[ExcludeFromCodeCoverage]
internal sealed class FakeFeedIteratorFactory : IFeedIteratorFactory
{
    public FeedIterator<T> GetFeedIterator<T>(IQueryable<T> query)
    {
        return new FakeFeedIterator<T>(query);
    }
}
