namespace Cosmos.Factories.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.Azure.Cosmos;

/// <summary>
/// This type exists so that it can be returned as a 'fake' feed response by the
/// <see cref="FakeFeedIterator{T}.ReadNextAsync(CancellationToken)"/> method.
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed class FakeFeedResponse<T> : FeedResponse<T>
{
    private readonly IQueryable<T> query;
    private readonly HttpStatusCode statusCode;

    public FakeFeedResponse(
        IQueryable<T> query,
        HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        this.query = query ?? throw new ArgumentNullException(nameof(query));
        this.statusCode = statusCode;
    }

    public override string ContinuationToken => string.Empty;

    public override int Count => this.query.Count();

    public override Headers Headers => new Headers();

    public override IEnumerable<T> Resource => this.query;

    public override HttpStatusCode StatusCode => this.statusCode;

    public override string IndexMetrics => throw new NotImplementedException();

    public override CosmosDiagnostics Diagnostics => throw new NotImplementedException();

    public override IEnumerator<T> GetEnumerator()
    {
        return this.query.GetEnumerator();
    }
}
