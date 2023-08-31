namespace Cosmos.Factories.Fakes;

using System.Diagnostics.CodeAnalysis;
using Cosmos.Factories.Extensions;
using Microsoft.Azure.Cosmos;

/// <summary>
/// This type exists so that it can be injected in unit tests as the response
/// of the <see cref="IFeedIteratorFactory.GetFeedIterator{T}(IQueryable{T})"/> method,
/// which will return a 'fake' Feed Iterator with just the items in the provided queryable.
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed class FakeFeedIterator<T> : FeedIterator<T>
{
    private readonly IQueryable<T> query;
    private bool hasMoreResults = true;

    public FakeFeedIterator(IQueryable<T> query)
    {
        this.query = query ?? throw new ArgumentNullException(nameof(query));
        this.hasMoreResults = this.query.Any();
    }

    public override bool HasMoreResults => this.hasMoreResults;

    public override Task<FeedResponse<T>> ReadNextAsync(CancellationToken cancellationToken = default)
    {
        var response = new FakeFeedResponse<T>(this.query);

        this.hasMoreResults = false;

        return response.AsTask();
    }
}
