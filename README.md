# Cosmos.Factories

This is a simple library which adds some 'factory' types for use with the Microsoft.Azure.Cosmos client library, to enable making methods mockable and testable, and avoid making network requests to actual Azure CosmosDb intances when running from within a test context.

In the [Microsoft.Azure.Cosmos SDK](https://github.com/Azure/azure-cosmos-dotnet-v3), the `.ToFeedIterator()` method is implemented as an extension method on the IQueryable type,
but throws an exception if the IQueryable you call it against is not actually an implementation of an internal type with no public constructor,
making it essentially impossible to use mocks in testing methods that do Cosmos queries using the feed iterator type (see [issue #893 here](https://github.com/Azure/azure-cosmos-dotnet-v3/issues/893)).

This library adds both an `ICosmosContainerFactory` and an `IFeedIteratorFactory` type, which enable injecting
an in-memory Cosmos container into service types that you want to test, so unit tests can actually test the functionality
in question without needing to talk to an actual Cosmos instance (or run the Cosmos emulator locally).

This approach is suggested on the Microsoft.Azure.Cosmos repo [here](https://github.com/Azure/azure-cosmos-dotnet-v3/issues/893#issuecomment-1060299574).

The version of this library is pinned to the version of the Cosmos SDK that it references.

## Usage

In your normal dependency registration (when not running in a test context):

```csharp
public void RegisterCosmos(IServiceCollection services)
{
  services.AddCosmosFactories();
  services.AddSingleton<CosmosClient>(provider =>
  {
    var builder = new CosmosClientBuilder();

    // configure your builder    
    return builder.Build();
  });
}
```

In your dependency registration when running in a test context,
you can use the .AddFakeCosmosClient() method to register an in-memory container factory
which will allow you to query, add, and remove items from the containers without
actually needing to talk to a Cosmos instance or run the Cosmos emulator:

```csharp
public void RegisterCosmos(IServiceCollection services)
{
  services.AddCosmosFactories();
  servics.AddFakeCosmosClient();
}
```


## Container implementation

Instead of directly injecting a `CosmosClient` instance where needed,
inject an `ICosmosContainerFactory` and an `IFeedIteratorFactory` instead:

```csharp
public sealed class UserService : IUserService
{
  private readonly Container _container;
  private readonly IFeedIteratorFactory _feedIteratorFactory;

  public UserService(
    ICosmosContainerFactory containerFactory,
    IFeedIteratorFactory feedIteratorFactory)
  {
    _container = containerFactory.GetContainer("MyDatabase", "Users");
    _feedIteratorFactory = feedIteratorFactory;
  }

  public async IAsyncEnumerable<User> GetActive([EnumeratorCancellation] CancellationToken ct)
  {
    var query = _container.GetItemLinqQueryable<User>()
      .Where(x => x.IsActive);

    using var iterator = _feedIterator.GetFeedIterator(queryable);

    while (iterator.HasMoreResults)
    {
      foreach (var user in await iterator.ReadNextAsync(ct))
      {
        yield return user;
      }
    }
  }
}
```

See the CosmosFactoryTests.cs class in the Tests project for more examples.