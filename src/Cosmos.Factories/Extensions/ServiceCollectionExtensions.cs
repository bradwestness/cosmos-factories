namespace Cosmos.Factories.Extensions;

using Microsoft.Azure.Cosmos;
using Cosmos.Factories.Fakes;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the default <see cref="ICosmosContainerFactory"/>
    /// and <see cref="IFeedIteratorFactory"/> implementationss
    /// into the provided <see cref="IServiceCollection"/>.
    /// </summary>
    public static IServiceCollection AddCosmosFactories(
        this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton<ICosmosContainerFactory, CosmosContainerFactory>();
        services.AddSingleton<IFeedIteratorFactory, FeedIteratorFactory>();

        return services;
    }

    /// <summary>
    /// Registers <see cref="FakeCosmosClient"/> as the singleton <see cref="CosmosClient"/> instance,
    /// so services using a CosmosClient can be executed from a test context without making
    /// network calls to an actual Azure CosmosDb instance.    /// 
    /// </summary>
    public static IServiceCollection AddFakeCosmosClient(
        this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton<IFeedIteratorFactory, FakeFeedIteratorFactory>();
        services.AddSingleton<CosmosClient, FakeCosmosClient>();

        return services;
    }
}
