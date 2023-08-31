namespace Cosmos.Factories.Fakes;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.Cosmos;

/// <summary>
/// This type exists so that it can be injected in a test context without
/// making actual network requests to an Azure CosmosDb instance.
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed class FakeCosmosClient : CosmosClient
{
    public FakeCosmosClient()
    {
    }

    public override Container GetContainer(string databaseId, string containerId)
    {
        if (string.IsNullOrWhiteSpace(databaseId))
        {
            throw new ArgumentException($"'{nameof(databaseId)}' cannot be null or whitespace.", nameof(databaseId));
        }

        if (string.IsNullOrWhiteSpace(containerId))
        {
            throw new ArgumentException($"'{nameof(containerId)}' cannot be null or whitespace.", nameof(containerId));
        }

        return new FakeCosmosContainer(containerId);
    }
}