namespace Cosmos.Factories.Tests;

using Cosmos.Factories.Extensions;
using Cosmos.Factories.Fakes;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;

public sealed class ServiceCollectionExtensionTests
{
    [Fact]
    public void AddCosmosFactories_RegistersExpectedServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<CosmosClient, FakeCosmosClient>();
        services.AddCosmosFactories();

        var provider = services.BuildServiceProvider();

        var cosmosContainerFactory = provider.GetRequiredService<ICosmosContainerFactory>();
        Assert.NotNull(cosmosContainerFactory);
        Assert.True(cosmosContainerFactory is CosmosContainerFactory);

        var feedIteratorFactory = provider.GetRequiredService<IFeedIteratorFactory>();
        Assert.NotNull(feedIteratorFactory);
        Assert.True(feedIteratorFactory is FeedIteratorFactory);
    }

    [Fact]
    public void AddTestCosmosClient_RegistersExpectedServices()
    {
        var services = new ServiceCollection();

        services.AddFakeCosmosClient();

        var provider = services.BuildServiceProvider();

        var cosmosClient = provider.GetRequiredService<CosmosClient>();

        Assert.NotNull(cosmosClient);
        Assert.True(cosmosClient is FakeCosmosClient);

        var container = cosmosClient.GetContainer("foo", "bar");
        Assert.NotNull(container);
        Assert.True(container is FakeCosmosContainer);
    }
}