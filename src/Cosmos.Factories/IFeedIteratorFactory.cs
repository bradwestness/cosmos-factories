namespace Cosmos.Factories;

using Cosmos.Factories.Fakes;
using Microsoft.Azure.Cosmos;

/// <summary>
/// This interface exists so that it may be injected into Cosmos container
/// implementations, and mocked in unit tests with an implementation of
/// <see cref="GetFeedIterator{T}(IQueryable{T})"/> that returns a <see cref="FakeFeedIterator{T}"/> instance.
/// </summary>
public interface IFeedIteratorFactory
{
    FeedIterator<T> GetFeedIterator<T>(IQueryable<T> query);
}
