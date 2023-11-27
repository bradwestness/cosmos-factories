namespace Cosmos.Factories.Fakes;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cosmos.Factories.Extensions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Scripts;

/// <summary>
/// This type exists so that it can be injected in a test context without
/// making actual network requests to an Azure CosmosDb instance.
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed class FakeCosmosContainer : Container
{
    private readonly ConcurrentDictionary<string, object> items = new ConcurrentDictionary<string, object>();
    private readonly string containerId;

    public FakeCosmosContainer(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace.", nameof(id));
        }

        this.containerId = id;
    }

    public override string Id => this.containerId;

    public override Database Database => throw new NotImplementedException();

    public override Conflicts Conflicts => throw new NotImplementedException();

    public override Scripts Scripts => throw new NotImplementedException();

    public override Task<ItemResponse<T>> CreateItemAsync<T>(T item, PartitionKey? partitionKey = null, ItemRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        var key = item.GetIdentifier();

        if (this.items.TryAdd(key, item))
        {
            return new FakeItemResponse<T>(item, HttpStatusCode.Created).AsTask();
        }

        return new FakeItemResponse<T>(HttpStatusCode.Conflict).AsTask();
    }

    public override Task<ResponseMessage> CreateItemStreamAsync(Stream streamPayload, PartitionKey partitionKey, ItemRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override TransactionalBatch CreateTransactionalBatch(PartitionKey partitionKey)
    {
        throw new NotImplementedException();
    }

    public override Task<ContainerResponse> DeleteContainerAsync(ContainerRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<ResponseMessage> DeleteContainerStreamAsync(ContainerRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<ItemResponse<T>> DeleteItemAsync<T>(string id, PartitionKey partitionKey, ItemRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        if (this.items.TryRemove(id, out var value)
            && value is T item)
        {
            return new FakeItemResponse<T>(item, HttpStatusCode.NoContent).AsTask();
        }

        return new FakeItemResponse<T>(HttpStatusCode.NotFound).AsTask();
    }

    public override Task<ResponseMessage> DeleteItemStreamAsync(string id, PartitionKey partitionKey, ItemRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override ChangeFeedEstimator GetChangeFeedEstimator(string processorName, Container leaseContainer)
    {
        throw new NotImplementedException();
    }

    public override ChangeFeedProcessorBuilder GetChangeFeedEstimatorBuilder(string processorName, ChangesEstimationHandler estimationDelegate, TimeSpan? estimationPeriod = null)
    {
        throw new NotImplementedException();
    }

    public override FeedIterator<T> GetChangeFeedIterator<T>(ChangeFeedStartFrom changeFeedStartFrom, ChangeFeedMode changeFeedMode, ChangeFeedRequestOptions? changeFeedRequestOptions = null)
    {
        throw new NotImplementedException();
    }

    public override ChangeFeedProcessorBuilder GetChangeFeedProcessorBuilder<T>(string processorName, ChangesHandler<T> onChangesDelegate)
    {
        throw new NotImplementedException();
    }

    public override ChangeFeedProcessorBuilder GetChangeFeedProcessorBuilder<T>(string processorName, ChangeFeedHandler<T> onChangesDelegate)
    {
        throw new NotImplementedException();
    }

    public override ChangeFeedProcessorBuilder GetChangeFeedProcessorBuilder(string processorName, ChangeFeedStreamHandler onChangesDelegate)
    {
        throw new NotImplementedException();
    }

    public override ChangeFeedProcessorBuilder GetChangeFeedProcessorBuilderWithManualCheckpoint<T>(string processorName, ChangeFeedHandlerWithManualCheckpoint<T> onChangesDelegate)
    {
        throw new NotImplementedException();
    }

    public override ChangeFeedProcessorBuilder GetChangeFeedProcessorBuilderWithManualCheckpoint(string processorName, ChangeFeedStreamHandlerWithManualCheckpoint onChangesDelegate)
    {
        throw new NotImplementedException();
    }

    public override FeedIterator GetChangeFeedStreamIterator(ChangeFeedStartFrom changeFeedStartFrom, ChangeFeedMode changeFeedMode, ChangeFeedRequestOptions? changeFeedRequestOptions = null)
    {
        throw new NotImplementedException();
    }

    public override Task<IReadOnlyList<FeedRange>> GetFeedRangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override IOrderedQueryable<T> GetItemLinqQueryable<T>(bool allowSynchronousQueryExecution = false, string? continuationToken = null, QueryRequestOptions? requestOptions = null, CosmosLinqSerializerOptions? linqSerializerOptions = null)
    {        
        return new EnumerableQuery<T>(this.items.Values.ToTyped<T>());
    }

    public override FeedIterator<T> GetItemQueryIterator<T>(QueryDefinition queryDefinition, string? continuationToken = null, QueryRequestOptions? requestOptions = null)
    {
        throw new NotImplementedException();
    }

    public override FeedIterator<T> GetItemQueryIterator<T>(string? queryText = null, string? continuationToken = null, QueryRequestOptions? requestOptions = null)
    {
        throw new NotImplementedException();
    }

    public override FeedIterator<T> GetItemQueryIterator<T>(FeedRange feedRange, QueryDefinition queryDefinition, string? continuationToken = null, QueryRequestOptions? requestOptions = null)
    {
        throw new NotImplementedException();
    }

    public override FeedIterator GetItemQueryStreamIterator(QueryDefinition queryDefinition, string? continuationToken = null, QueryRequestOptions? requestOptions = null)
    {
        throw new NotImplementedException();
    }

    public override FeedIterator GetItemQueryStreamIterator(string? queryText = null, string? continuationToken = null, QueryRequestOptions? requestOptions = null)
    {
        throw new NotImplementedException();
    }

    public override FeedIterator GetItemQueryStreamIterator(FeedRange feedRange, QueryDefinition queryDefinition, string continuationToken, QueryRequestOptions? requestOptions = null)
    {
        throw new NotImplementedException();
    }

    public override Task<ItemResponse<T>> PatchItemAsync<T>(string id, PartitionKey partitionKey, IReadOnlyList<PatchOperation> patchOperations, PatchItemRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<ResponseMessage> PatchItemStreamAsync(string id, PartitionKey partitionKey, IReadOnlyList<PatchOperation> patchOperations, PatchItemRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<ContainerResponse> ReadContainerAsync(ContainerRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<ResponseMessage> ReadContainerStreamAsync(ContainerRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<ItemResponse<T>> ReadItemAsync<T>(string id, PartitionKey partitionKey, ItemRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        if (this.items.TryGetValue(id, out object value)
            && value is T item)
        {
            return new FakeItemResponse<T>(item).AsTask();
        }

        return new FakeItemResponse<T>(HttpStatusCode.NotFound).AsTask();
    }

    public override Task<ResponseMessage> ReadItemStreamAsync(string id, PartitionKey partitionKey, ItemRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<FeedResponse<T>> ReadManyItemsAsync<T>(IReadOnlyList<(string id, PartitionKey partitionKey)> items, ReadManyRequestOptions? readManyRequestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<ResponseMessage> ReadManyItemsStreamAsync(IReadOnlyList<(string id, PartitionKey partitionKey)> items, ReadManyRequestOptions? readManyRequestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<int?> ReadThroughputAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<ThroughputResponse> ReadThroughputAsync(RequestOptions requestOptions, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<ContainerResponse> ReplaceContainerAsync(ContainerProperties containerProperties, ContainerRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<ResponseMessage> ReplaceContainerStreamAsync(ContainerProperties containerProperties, ContainerRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<ItemResponse<T>> ReplaceItemAsync<T>(T item, string id, PartitionKey? partitionKey = null, ItemRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        if (this.items.ContainsKey(id))
        {
            this.items.AddOrUpdate(id, item, (key, oldValue) => item);
            return new FakeItemResponse<T>(item).AsTask();
        }

        return new FakeItemResponse<T>(HttpStatusCode.NotFound).AsTask();
    }

    public override Task<ResponseMessage> ReplaceItemStreamAsync(Stream streamPayload, string id, PartitionKey partitionKey, ItemRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<ThroughputResponse> ReplaceThroughputAsync(int throughput, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<ThroughputResponse> ReplaceThroughputAsync(ThroughputProperties throughputProperties, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<ItemResponse<T>> UpsertItemAsync<T>(T item, PartitionKey? partitionKey = null, ItemRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        var id = item.GetIdentifier();
        this.items.AddOrUpdate(id, item, (key, oldValue) => item);
        return new FakeItemResponse<T>(item).AsTask();
    }

    public override Task<ResponseMessage> UpsertItemStreamAsync(Stream streamPayload, PartitionKey partitionKey, ItemRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}