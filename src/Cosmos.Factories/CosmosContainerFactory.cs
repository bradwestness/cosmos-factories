namespace Cosmos.Factories;

using Microsoft.Azure.Cosmos;
public sealed class CosmosContainerFactory : ICosmosContainerFactory
{
    private readonly CosmosClient cosmosClient;

    public CosmosContainerFactory(CosmosClient cosmosClient)
    {
        this.cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));
    }

    public Container GetContainer(string databaseId, string containerId)
    {
        return this.cosmosClient.GetContainer(databaseId, containerId);
    }
}