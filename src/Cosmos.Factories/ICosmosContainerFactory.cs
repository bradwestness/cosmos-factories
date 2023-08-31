namespace Cosmos.Factories;

using Microsoft.Azure.Cosmos;
public interface ICosmosContainerFactory
{
    Container GetContainer(string databaseId, string containerId);
}