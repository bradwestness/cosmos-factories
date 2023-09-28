namespace Cosmos.Factories.Tests.Extensions;
using System.Collections.Generic;
using System.Linq;
using Cosmos.Factories.Extensions;

public sealed class EnumerableExtensionsTests
{
    [Fact]
    public void ToTyped_Generic_Null_ReturnsEmptyEnumerable()
    {
        var input = (IEnumerable<object>)null!;

        var actual = input.ToTyped<object, int>();

        Assert.NotNull(actual);
        Assert.Empty(actual);
    }

    [Fact]
    public void ToTyped_Generic_ReturnsValuesOfType()
    {
        var input = new object[]
        {
            1,
            new object(),
            2,
            "string",
            3
        };

        var actual = input.ToTyped<object, int>();

        Assert.NotNull(actual);
        Assert.Equal(3, actual.Count());
        Assert.Equal(1, actual.ElementAt(0));
        Assert.Equal(2, actual.ElementAt(1));
        Assert.Equal(3, actual.ElementAt(2));
    }

    [Fact]
    public void ToTyped_Object_Null_ReturnsEmptyEnumerable()
    {
        var input = (IEnumerable<object>)null!;

        var actual = input.ToTyped<int>();

        Assert.NotNull(actual);
        Assert.Empty(actual);
    }

    [Fact]
    public void ToTyped_Object_ReturnsValuesOfType()
    {
        var input = new object[]
        {
            1,
            new object(),
            2,
            "string",
            3
        };

        var actual = input.ToTyped<int>();

        Assert.NotNull(actual);
        Assert.Equal(3, actual.Count());
        Assert.Equal(1, actual.ElementAt(0));
        Assert.Equal(2, actual.ElementAt(1));
        Assert.Equal(3, actual.ElementAt(2));
    }
}
