namespace Cosmos.Factories.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.Azure.Cosmos;

/// <summary>
/// This type exists so that it can be returned as a 'fake' item response by the
/// <see cref="FakeCosmosContainer"/> class.
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed class FakeItemResponse<T> : ItemResponse<T>
{
    private readonly T item;
    private readonly HttpStatusCode statusCode;

    public FakeItemResponse(HttpStatusCode statusCode)
    {
        this.item = default!;
        this.statusCode = statusCode;
    }

    public FakeItemResponse(T item, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        this.item = item;
        this.statusCode = statusCode;
    }

    public override double RequestCharge => 0;

    public override T Resource => this.item;

    public override HttpStatusCode StatusCode => this.statusCode;
}
