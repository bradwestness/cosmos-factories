namespace Cosmos.Factories.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.Factories.Extensions;
using Cosmos.Factories.Fakes;
using Microsoft.Azure.Cosmos;

public sealed class ResponseExtensionsTests
{
    [Fact]
    public async Task ItemResponse_AsTask_Null_ReturnsExpected()
    {
        var value = (ItemResponse<object>)null!;
        var task = value.AsTask();

        Assert.NotNull(task);
        Assert.True(task.IsCompleted);
        Assert.True(task.IsCompletedSuccessfully);

        var actual = await task;

        Assert.Null(actual);
    }

    [Fact]
    public async Task ItemResponse_AsTask_ReturnsExpected()
    {
        var value = new FakeItemResponse<object>(new object(), System.Net.HttpStatusCode.OK);
        var task = value.AsTask();

        Assert.NotNull(task);
        Assert.True(task.IsCompleted);
        Assert.True(task.IsCompletedSuccessfully);

        var actual = await task;

        Assert.NotNull(actual);
        Assert.Equal(value, actual);
    }

    [Fact]
    public async Task FeedResponse_AsTask_Null_ReturnsExpected()
    {
        var value = (FeedResponse<object>)null!;
        var task = value.AsTask();

        Assert.NotNull(task);
        Assert.True(task.IsCompleted);
        Assert.True(task.IsCompletedSuccessfully);

        var actual = await task;

        Assert.Null(actual);
    }

    [Fact]
    public async Task FeedResponse_AsTask_ReturnsExpected()
    {
        var queryable = Enumerable.Empty<object>().AsQueryable();
        var value = new FakeFeedResponse<object>(queryable, System.Net.HttpStatusCode.OK);
        var task = value.AsTask();

        Assert.NotNull(task);
        Assert.True(task.IsCompleted);
        Assert.True(task.IsCompletedSuccessfully);

        var actual = await task;

        Assert.NotNull(actual);
        Assert.Equal(value, actual);
    }
}
