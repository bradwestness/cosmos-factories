namespace Cosmos.Factories.Tests.Fakes;

using System.Net;
using System.Threading.Tasks;
using Cosmos.Factories.Fakes;
using Microsoft.Azure.Cosmos;

public sealed class FakeContainerTests
{
    [Fact]
    public async Task CreateItemAsync_ReturnsCreated()
    {
        var container = new FakeCosmosContainer("FakeContainer");
        Assert.NotNull(container);

        var expected = new Person("foo", "Foo", "Bar");

        var response = await container.CreateItemAsync(expected);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateItemAsync_ItemExists_ReturnsConflict()
    {
        var container = new FakeCosmosContainer("FakeContainer");
        Assert.NotNull(container);

        var expected = new Person("foo", "Foo", "Bar");

        var okResponse = await container.CreateItemAsync(expected);
        Assert.NotNull(okResponse);
        Assert.Equal(HttpStatusCode.Created, okResponse.StatusCode);

        // trying to upsert the item again
        // with the same ID should result in a conflict
        var conflictResponse = await container.CreateItemAsync(expected);
        Assert.NotNull(conflictResponse);
        Assert.Equal(HttpStatusCode.Conflict, conflictResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteItemAsync_ItemExists_ReturnsNoContent()
    {
        var container = new FakeCosmosContainer("FakeContainer");
        Assert.NotNull(container);

        var expected = new Person("foo", "Foo", "Bar");

        var createResponse = await container.CreateItemAsync(expected);
        Assert.NotNull(createResponse);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var deleteResponse = await container.DeleteItemAsync<Person>(expected.id, PartitionKey.None);
        Assert.NotNull(deleteResponse);
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteItemAsync_ItemDoesNotExist_ReturnsNotFound()
    {
        var container = new FakeCosmosContainer("FakeContainer");
        Assert.NotNull(container);

        var deleteResponse = await container.DeleteItemAsync<Person>("bar", PartitionKey.None);
        Assert.NotNull(deleteResponse);
        Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task GetItemLinqQueryable_ReturnsExpected()
    {
        var container = new FakeCosmosContainer("FakeContainer");
        Assert.NotNull(container);

        var expected = new Person("foo", "Foo", "Bar");

        var response = await container.UpsertItemAsync(expected);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var queryable = container.GetItemLinqQueryable<Person>();
        Assert.NotNull(queryable);
        Assert.Contains(expected, queryable);
    }

    [Fact]
    public async Task ReadItemAsync_ItemExists_ReturnsOk()
    {
        var container = new FakeCosmosContainer("FakeContainer");
        Assert.NotNull(container);

        var expected = new Person("foo", "Foo", "Bar");

        var upsertResponse = await container.UpsertItemAsync(expected);
        Assert.NotNull(upsertResponse);
        Assert.Equal(HttpStatusCode.OK, upsertResponse.StatusCode);

        var readResponse = await container.ReadItemAsync<Person>("foo", PartitionKey.None);
        Assert.NotNull(readResponse);
        Assert.Equal(HttpStatusCode.OK, readResponse.StatusCode);
    }

    [Fact]
    public async Task ReadItemAsync_ItemDoesNotExist_ReturnsNotFound()
    {
        var container = new FakeCosmosContainer("FakeContainer");
        Assert.NotNull(container);

        var response = await container.ReadItemAsync<Person>("foo", PartitionKey.None);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ReplaceItemAsync_ItemExists_ReturnsOk()
    {
        var container = new FakeCosmosContainer("FakeContainer");
        Assert.NotNull(container);

        var original = new Person("foo", "Foo", "Bar");

        var upsertResponse = await container.UpsertItemAsync(original);
        Assert.NotNull(upsertResponse);
        Assert.Equal(HttpStatusCode.OK, upsertResponse.StatusCode);

        // id is the same
        var replacement = new Person("foo", "Fizz", "Buzz");
        var replaceResponse = await container.ReplaceItemAsync(replacement, replacement.id);
        Assert.NotNull(replaceResponse);
        Assert.Equal(HttpStatusCode.OK, replaceResponse.StatusCode);

        // verify that we can now read the replacement item from the container
        var readResponse = await container.ReadItemAsync<Person>(replacement.id, PartitionKey.None);
        Assert.NotNull(readResponse);
        Assert.Equal(HttpStatusCode.OK, readResponse.StatusCode);
        Assert.Equal(replacement.id, readResponse.Resource.id);
        Assert.Equal(replacement.FirstName, readResponse.Resource.FirstName);
        Assert.Equal(replacement.LastName, readResponse.Resource.LastName);
    }

    [Fact]
    public async Task ReplaceItemAsync_ItemDoesNotExist_ReturnsNotFound()
    {
        var container = new FakeCosmosContainer("FakeContainer");
        Assert.NotNull(container);

        var expected = new Person("foo", "Foo", "Bar");
        var response = await container.ReplaceItemAsync(expected, expected.id);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpsertItemAsync_ItemDoesNotExist_ReturnsOk()
    {
        var container = new FakeCosmosContainer("FakeContainer");
        Assert.NotNull(container);

        var expected = new Person("foo", "Foo", "Bar");
        var upsertResponse = await container.UpsertItemAsync(expected);
        Assert.NotNull(upsertResponse);
        Assert.Equal(HttpStatusCode.OK, upsertResponse.StatusCode);

        // verify that we can now read the upserted item from the container
        var readResponse = await container.ReadItemAsync<Person>(expected.id, PartitionKey.None);
        Assert.NotNull(readResponse);
        Assert.Equal(HttpStatusCode.OK, readResponse.StatusCode);
        Assert.Equal(expected.id, readResponse.Resource.id);
        Assert.Equal(expected.FirstName, readResponse.Resource.FirstName);
        Assert.Equal(expected.LastName, readResponse.Resource.LastName);
    }

    [Fact]
    public async Task UpsertItemAsync_ItemExists_ReturnsOk()
    {
        var container = new FakeCosmosContainer("FakeContainer");
        Assert.NotNull(container);

        var original = new Person("foo", "Foo", "Bar");
        var originalResposne = await container.UpsertItemAsync(original);
        Assert.NotNull(originalResposne);
        Assert.Equal(HttpStatusCode.OK, originalResposne.StatusCode);

        // id is the same
        var replacement = new Person("foo", "Fizz", "Buzz");
        var replacementResponse = await container.UpsertItemAsync(replacement);
        Assert.NotNull(replacementResponse);
        Assert.Equal(HttpStatusCode.OK, replacementResponse.StatusCode);

        // verify that we can now read the replacement item from the container
        var readResponse = await container.ReadItemAsync<Person>(replacement.id, PartitionKey.None);
        Assert.NotNull(readResponse);
        Assert.Equal(HttpStatusCode.OK, readResponse.StatusCode);
        Assert.Equal(replacement.id, readResponse.Resource.id);
        Assert.Equal(replacement.FirstName, readResponse.Resource.FirstName);
        Assert.Equal(replacement.LastName, readResponse.Resource.LastName);
    }

    private record Person(string id, string FirstName, string LastName);
}
